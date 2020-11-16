using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SerialSample
{
    public class AzureIoTHubService
    {
        private DeviceClient _deviceClient;

        public AzureIoTHubService()
        {
            _deviceClient = DeviceClient.CreateFromConnectionString("HostName=MyEnergy.azure-devices.net;DeviceId=RaspberryPi3;SharedAccessKey=hGtOd0w0PXylzuAsOVo+Vka+mgWHIqe5UgFbHWQmmWI=", TransportType.Mqtt);
        }

        public async Task<bool> SendDataToAzure(string data)
        {
            var messageString = JsonConvert.SerializeObject(data);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await _deviceClient.SendEventAsync(message);

            Debug.WriteLine("{0} > Sending telemetry: {1}", DateTime.Now, messageString);
            return true;
        }
    }
}
