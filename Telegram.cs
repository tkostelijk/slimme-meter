using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SerialSample
{
    public class Telegram
    {
        private DateTime timestamp;
        private Double kwh1;
        private Double kwh2;
        private Int16 tarrif;
        private Double kw1;
        private Double kw2;
        private Int32 pfailures; //power failures
        private Int32 lpfailures; // long power failures
        private DateTime pfailuretimestamp;
        private TimeSpan pfailureduration;
        private Int32 voltsags;
        private Int32 voltswells;
        private Double voltage;
        private Int16 current;
        private Double totalkw;
        private DateTime gastimestamp;
        private Double gasvolume;

        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }
        public Double kWh1
        {
            get
            {
                return kwh1;
            }
            set
            {
                kwh1 = value;
            }
        }
        public Double kWh2
        {
            get
            {
                return kwh2;
            }
            set
            {
                kwh2 = value;
            }
        }
        public Int16 Tarrif
        {
            get
            {
                return tarrif;
            }
            set
            {
                tarrif = value;
            }
        }
        public Double kW1
        {
            get
            {
                return kw1;
            }
            set
            {
                kw1 = value;
            }
        }
        public Double kW2
        {
            get
            {
                return kw2;
            }
            set
            {
                kw2 = value;
            }
        }
        public Int32 PowerFailures //power failures
        {
            get
            {
                return pfailures;
            }
            set
            {
                pfailures = value;
            }
        }
        public Int32 LongPowerFailures // long power failures
        {
            get
            {
                return lpfailures;
            }
            set
            {
                lpfailures = value;
            }
        }
        public DateTime PowerFailureTimeStamp
        {
            get
            {
                return pfailuretimestamp;
            }
            set
            {
                pfailuretimestamp = value;
            }
        }
        public TimeSpan PowerFailureDuration
        {
            get
            {
                return pfailureduration;
            }
            set
            {
                pfailureduration = value;
            }
        }
        public Int32 VoltSags
        {
            get
            {
                return voltsags;
            }
            set
            {
                voltsags = value;
            }
        }
        public Int32 VoltSwells
        {
            get
            {
                return voltswells;
            }
            set
            {
                voltswells = value;
            }
        }
        public Double Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                voltage = value;
            }
        }
        public Int16 Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
            }
        }
        public Double TotalKw
        {
            get
            {
                return totalkw;
            }
            set
            {
                totalkw = value;
            }
        }
        public DateTime GasTimeStamp
        {
            get
            {
                return gastimestamp;
            }
            set
            {
                gastimestamp = value;
            }
        }
        public Double GasVolume
        {
            get
            {
                return gasvolume;
            }
            set
            {
                gasvolume = value;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sTimeStamp"></param>
        /// <param name="sKWh1"></param>
        /// <param name="sKWh2"></param>
        /// <param name="sKW"></param>
        public Telegram(string sTelegram)
        {
            //logic to convert strings to Time and Doubles.
            string telegramLine;
            StringReader strTelegram = new StringReader(sTelegram);
            while (true)
            {
                telegramLine = strTelegram.ReadLine();
                if (telegramLine != null)
                {
                    // Verwerk telegram regel
                    // Get first match.
                    Match telegramMatchID = Regex.Match(telegramLine, @"[0-9]\-[0-9]\:\d+.\d+.\d+");
                    if (telegramMatchID.Success)
                    {
                        string telegramID = telegramMatchID.Value;
                        switch (telegramID)
                        {
                            case "0-0:1.0.0":
                                timestamp = Utils.GetDateTime(telegramLine);
                                break;
                            case "1-0:1.8.1":
                                kwh1 = Utils.GetkWh(telegramLine);
                                break;
                            case "1-0:1.8.2":
                                kwh2 = Utils.GetkWh(telegramLine);
                                break;
                            case "0-0:96.14.0":
                                kwh1 = Utils.GetNumber4(telegramLine);
                                break;
                            case "1-0:1.7.0":
                                kw1 = Utils.GetkW(telegramLine);
                                break;
                            case "1-0:2.7.0":
                                kw2 = Utils.GetkW(telegramLine);
                                break;
                            case "0-0:96.7.21":
                                pfailures = Utils.GetNumber5(telegramLine);
                                break;
                            case "0-0:96.7.9":
                                lpfailures = Utils.GetNumber5(telegramLine);
                                break;
                            case "1-0:99.97.0":
                                pfailuretimestamp = Utils.GetDateTime(telegramLine);
                                pfailureduration = Utils.GetDuration(telegramLine);
                                break;
                            case "1-0:32.32.0":
                                voltsags = Utils.GetNumber5(telegramLine);
                                break;
                            case "1-0:32.36.0":
                                voltswells = Utils.GetNumber5(telegramLine);
                                break;
                            case "1-0:32.7.0":
                                voltage = Utils.GetVoltage(telegramLine);
                                break;
                            case "1-0:31.7.0":
                                current = Utils.GetNumber3(telegramLine);
                                break;
                            case "1-0:21.7.0":
                                totalkw = Utils.GetkW(telegramLine);
                                break;
                            case "0-1:24.2.1":
                                gastimestamp = Utils.GetDateTime(telegramLine);
                                gasvolume = Utils.GetGasVolume(telegramLine);
                                break;
                            default:
                                // no match
                                break;
                        }
                    }

                }
                else
                {
                    break;
                }
            }
        }
    }
}
