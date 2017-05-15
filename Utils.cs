using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialSample
{
    public class Utils
    {
        public static DateTime GetDateTime(string sDateTime)
        {

            DateTime dtTimeStamp = new DateTime();

            string regularExpressionPattern = @"\d{12}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sDateTime))
            {
                dtTimeStamp = Convert.ToDateTime(m.Value.Substring(2, 2) + '/' + m.Value.Substring(4, 2) + '/' + m.Value.Substring(0, 2) + ' ' + m.Value.Substring(6, 2) + ':' + m.Value.Substring(8, 2) + ':' + m.Value.Substring(10, 2));
            }

            return dtTimeStamp;
        }
        public static TimeSpan GetDuration(string sSeconds)
        {

            TimeSpan tsDuration = new TimeSpan();

            string regularExpressionPattern = @"\d{10}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sSeconds))
            {
                tsDuration = TimeSpan.FromSeconds(Convert.ToDouble(m.Value));
            }

            return tsDuration;
        }
        public static Double GetkWh(string sKWh)
        {

            Double dKWh = new Double();

            string regularExpressionPattern = @"\d{6}\.\d{3}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sKWh))
            {
                dKWh = Convert.ToDouble(m.Value);
            }

            return dKWh;
        }
        public static Double GetkW(string sKW)
        {

            Double dKW = new Double();

            string regularExpressionPattern = @"\d{2}\.\d{3}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sKW))
            {
                dKW = Convert.ToDouble(m.Value);
            }

            return dKW;
        }
        public static Double GetVoltage(string sVoltage)
        {

            Double dVoltage = new Double();

            string regularExpressionPattern = @"\d{3}\.\d{1}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sVoltage))
            {
                dVoltage = Convert.ToDouble(m.Value);
            }

            return dVoltage;
        }
        public static Int16 GetNumber3(string sNumber)
        {

            Int16 iNumber = 0;

            string regularExpressionPattern = @"\d{3}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sNumber))
            {
                iNumber = Convert.ToInt16(m.Value);
            }

            return iNumber;
        }
        public static Int16 GetNumber4(string sNumber)
        {

            Int16 iNumber = 0;

            string regularExpressionPattern = @"\d{4}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sNumber))
            {
                iNumber = Convert.ToInt16(m.Value);
            }

            return iNumber;
        }
        public static Int32 GetNumber5(string sNumber)
        {

            Int32 iNumber = 0;

            string regularExpressionPattern = @"\d{5}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sNumber))
            {
                iNumber = Convert.ToInt16(m.Value);
            }

            return iNumber;
        }
        public static Double GetGasVolume(string sGasVolume)
        {

            Double dGasVolume = new Double();

            string regularExpressionPattern = @"\d{5}\.\d{3}";

            Regex re = new Regex(regularExpressionPattern);

            foreach (Match m in re.Matches(sGasVolume))
            {
                dGasVolume = Convert.ToDouble(m.Value);
            }

            return dGasVolume;
        }
    }
}
