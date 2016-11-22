using hdhr_vintage.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.DataAccess
{
    public static class DatabaseCommand
    {

        public static void CreateDatabase()
        {
            string filename = "hdhr-vintage.db";

            if(File.Exists(filename))
            {
                File.Delete(filename);
            }
            SQLiteConnection.CreateFile(filename);
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + filename))
            {
                string sql;
                SQLiteCommand command;

                conn.Open();

                sql = "CREATE TABLE `Device` ( `DeviceID` TEXT NOT NULL UNIQUE, `IP` TEXT, PRIMARY KEY(`DeviceID`) )";
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();



                conn.Close();
            }
        }

        public static Device CreateDevice(string unparsedText)
        {
            //example
            //hdhomerun device 10183772 found at 192.168.1.184

            var device = new Device();
            if(unparsedText.ToLower().StartsWith("hdhomerun device ") && unparsedText.ToLower().Contains(" found at "))
            {
                using (var context = new SQLiteContext())
                {
                    //hdhomerun_config docs strongly imply the device id is always 8 characters
                    //but this parsing could be improved anyway
                    string deviceID = unparsedText.Substring("hdhomerun device ".Length, 8);
                    string ip = unparsedText.Substring(unparsedText.Trim().LastIndexOf(' ') + 1);

                    device.DeviceID = deviceID;
                    device.IP = ip;  

                    context.Device.Add(device);
                    context.SaveChanges();
                }
            }
            else
            {
                //
            }
            return device;
        }

        public static List<Device> GetDevices()
        {
            using (var context = new SQLiteContext())
            {
                return context.Device.ToList();
            }
        }

    }
}
