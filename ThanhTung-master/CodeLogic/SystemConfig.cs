using System;
using System.Configuration;

namespace QuanLyHoaDon.CodeLogic
{
    public class SystemConfig
    {
        public static string GetValueByKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key]; ;
            }
            catch (Exception)
            {

                return "0";
            }
        }

        public static string GetConnectString(string key)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception)
            {

                return "NULL";
            }
        }
    }
}
