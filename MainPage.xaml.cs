// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SerialSample
{    
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Private variables
        /// </summary>
        private SerialDevice serialPort = null;
        DataReader dataReaderObject = null;

        private ObservableCollection<DeviceInformation> listOfDevices;
        private CancellationTokenSource ReadCancellationTokenSource;
        private Double OldGasVolume;
        private DateTime OldGasTimeStamp;
        private AzureIoTHubService _azureIoTHubService;

        public MainPage()
        {
            this.InitializeComponent();
            _azureIoTHubService = new AzureIoTHubService();
            comPortInput.IsEnabled = false;
            listOfDevices = new ObservableCollection<DeviceInformation>();
            ListAvailablePorts();
            StartWithFixedID();
        }

        /// <summary>
        /// ListAvailablePorts
        /// - Use SerialDevice.GetDeviceSelector to enumerate all serial devices
        /// - Attaches the DeviceInformation to the ListBox source so that DeviceIds are displayed
        /// </summary>
        private async void ListAvailablePorts()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);

                status.Text = "Select a device and connect";

                for (int i = 0; i < dis.Count; i++)
                {
                    listOfDevices.Add(dis[i]);
                }

                DeviceListSource.Source = listOfDevices;
                comPortInput.IsEnabled = true;
                ConnectDevices.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }
        }

        /// <summary>
        /// comPortInput_Click: Action to take when 'Connect' button is clicked
        /// - Get the selected device index and use Id to create the SerialDevice object
        /// - Configure default settings for the serial port
        /// - Create the ReadCancellationTokenSource token
        /// - Start listening on the serial port input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void comPortInput_Click(object sender, RoutedEventArgs e)
        {
            var selection = ConnectDevices.SelectedItems;

            if (selection.Count <= 0)
            {
                status.Text = "Select a device and connect";
                return;
            }

            DeviceInformation entry = (DeviceInformation)selection[0];

            // \\?\FTDIBUS#VID_0403+PID_6001+P11A5YWMA#0000#{86e0d1e0-8089-11d09ce4-08003e301f73}       
            // string entryID = "\\\\?\\FTDIBUS#VID_0403+PID_6001+P11A5YWMA#0000#{86e0d1e0-8089-11d09ce4-08003e301f73}";

            string entryID = entry.Id;


            try
            {
                //serialPort = await SerialDevice.FromIdAsync(entry.Id);
                serialPort = await SerialDevice.FromIdAsync(entryID);

                // Disable the 'Connect' button 
                comPortInput.IsEnabled = false;

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(500);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(500);                
                serialPort.BaudRate = 115200;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.XOnXOff;

                // Display configured settings
                status.Text = "Serial port ";
                status.Text += entry.Id;
                status.Text += " configured successfully: ";
                status.Text += serialPort.BaudRate + "-";
                status.Text += serialPort.DataBits + "-";
                status.Text += serialPort.Parity.ToString() + "-";
                status.Text += serialPort.StopBits + "-";
                status.Text += serialPort.Handshake; // Maybe .TosTring() is needed.

                // Set the RcvdText field to invoke the TextChanged callback
                // The callback launches an async Read task to wait for data
                //status.Text = "Waiting for data...";

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();

                
                Listen();
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
                comPortInput.IsEnabled = true;
            }
        }

        private async void StartWithFixedID()
        {
            var selection = ConnectDevices.SelectedItems;

            //        if (selection.Count <= 0)
            //        {
            //            status.Text = "Select a device and connect";
            //            return;
            //        }

            //        DeviceInformation entry = (DeviceInformation)selection[0];

          
            // \\?\FTDIBUS#VID_0403+PID_6001+P11A5YWMA#0000#{86e0d1e0-8089-11d0-9ce4-08003e301f73}
            string entryID = "\\\\?\\FTDIBUS#VID_0403+PID_6001+P11A5YWMA#0000#{86e0d1e0-8089-11d0-9ce4-08003e301f73}";

            //string entryID = entry.Id;


            try
            {
                //serialPort = await SerialDevice.FromIdAsync(entry.Id);
                serialPort = await SerialDevice.FromIdAsync(entryID);

                // Disable the 'Connect' button 
                comPortInput.IsEnabled = false;

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(500);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(500);
                serialPort.BaudRate = 115200;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.XOnXOff;

                // Display configured settings
                status.Text = "Serial port ";
       //         status.Text += entry.Id;
                status.Text += " configured successfully: ";
                status.Text += serialPort.BaudRate + "-";
                status.Text += serialPort.DataBits + "-";
                status.Text += serialPort.Parity.ToString() + "-";
                status.Text += serialPort.StopBits + "-";
                status.Text += serialPort.Handshake; // Maybe .TosTring() is needed.

                // Set the RcvdText field to invoke the TextChanged callback
                // The callback launches an async Read task to wait for data
                //status.Text = "Waiting for data...";

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();


                Listen();
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
                comPortInput.IsEnabled = true;
            }
        }




        /// <summary>
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException tce) 
            {
                status.Text = "Reading task was cancelled, closing device and cleaning up " + tce.Message;
                CloseDevice();            
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        /// <summary>
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            // uint ReadBufferLength = 589;
            uint ReadBufferLength = 768;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;


            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);


            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                string readString = dataReaderObject.ReadString(bytesRead);
                Telegram telegram = new Telegram(readString);

                if(OldGasVolume == 0)
                {
                    OldGasVolume = telegram.GasVolume;
                    OldGasTimeStamp = telegram.GasTimeStamp;
                }
                
                rcvdText.Text = telegram.TimeStamp.ToString();
                Watts.Text = String.Format("{0} Watt",telegram.TotalKw * 1000);
                KostHour.Text = String.Format("€ {0:N2}/uur", Math.Round(telegram.TotalKw * 0.21733,2));
                kiloWattHour.Text = String.Format("{0:N3} kWh",telegram.kWh1 + telegram.kWh2);
                Gas.Text = String.Format("{0}\r\n {1:N3} m3",telegram.GasTimeStamp,telegram.GasVolume);

                if (OldGasTimeStamp != telegram.GasTimeStamp)
                {
                    GasUsage.Text = String.Format("{0:N3} m3 in the last {1}\r\n€ {2:n2}", telegram.GasVolume - OldGasVolume, telegram.GasTimeStamp - OldGasTimeStamp,(telegram.GasVolume - OldGasVolume)* 0.72113);
                    OldGasTimeStamp = telegram.GasTimeStamp;
                    OldGasVolume = telegram.GasVolume;
                }
                // Send Telegram info to IoT hub
                var jsonTelegram = JsonConvert.SerializeObject(telegram);
                await _azureIoTHubService.SendDataToAzure(jsonTelegram);

            }            
        }

        /// <summary>
        /// CancelReadTask:
        /// - Uses the ReadCancellationTokenSource to cancel read operations
        /// </summary>
        private void CancelReadTask()
        {         
            if (ReadCancellationTokenSource != null)
            {
                if (!ReadCancellationTokenSource.IsCancellationRequested)
                {
                    ReadCancellationTokenSource.Cancel();
                }
            }         
        }

        /// <summary>
        /// CloseDevice:
        /// - Disposes SerialDevice object
        /// - Clears the enumerated device Id list
        /// </summary>
        private void CloseDevice()
        {            
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;

            comPortInput.IsEnabled = true;
            //rcvdText.Text = "";
            listOfDevices.Clear();               
        }

        /// <summary>
        /// closeDevice_Click: Action to take when 'Disconnect and Refresh List' is clicked on
        /// - Cancel all read operations
        /// - Close and dispose the SerialDevice object
        /// - Enumerate connected devices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeDevice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                status.Text = "";
                CancelReadTask();
                CloseDevice();
                ListAvailablePorts();
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }          
        }
    }
}
