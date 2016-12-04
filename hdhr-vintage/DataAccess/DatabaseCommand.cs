using hdhr_vintage.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace hdhr_vintage.DataAccess
{
    public static class DatabaseCommand
    {

        public static T CreateEntity<T>(T entity) where T : class
        {
            using (var context = new SQLiteContext())
            {
                try
                {
                    context.Set<T>().Add(entity);
                    context.SaveChanges();
                    return entity;
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string stop = ex.Message;
                    throw;
                }
                
            }
        }

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

                sql = @"CREATE TABLE `Tuner` (`TunerID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `TunerNumber` INTEGER NOT NULL, `DeviceID` TEXT NOT NULL, 'ChannelMap' TEXT NOT NULL,
	                    FOREIGN KEY(`DeviceID`) REFERENCES Device);";
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();

                sql = @"CREATE TABLE `Channel` (`ChannelID` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `ChannelNumber` INT NOT NULL, `TunerID` INT NOT NULL,
	                    FOREIGN KEY(`TunerID`) REFERENCES Tuner);";
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();

                sql = @"CREATE TABLE `Program` (`ProgramID` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `ProgramNumber` TEXT NOT NULL, 
                        `FriendlyChannelNumber` TEXT NOT NULL, `CallSign` TEXT NULL, `IsFavorite` INTEGER NOT NULL, `ChannelID` INT NOT NULL,
	                    FOREIGN KEY(`ChannelID`) REFERENCES Channel);";
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
                return context.Device
                    .Include(i => i.Tuners)
                    .ToList();
            }
        }

        public static List<Tuner> GetTuners()
        {
            using (var context = new SQLiteContext())
            {
                return context.Tuner.ToList();
            }
        }

        public static Tuner GetTuner(string deviceID, int tunerNumber)
        {
            using (var context = new SQLiteContext())
            {
                return context.Tuner
                    .Where(w => w.DeviceID == deviceID)
                    .Where(w => w.TunerNumber == tunerNumber)
                    .FirstOrDefault();
            }
        }

        public static List<Program> GetPrograms(int tunerID)
        {
            using (var context = new SQLiteContext())
            {
                var channels = context.Channel
                    .Include(i => i.Programs)
                    .Include(i => i.Tuner)
                    .Where(w => w.TunerID == tunerID)
                    .ToList();

                var programs = new List<Program>();
                channels.ForEach(x => programs.AddRange(x.Programs));

                return programs
                    .OrderBy(o => o.FriendlyChannelMain)
                    .ThenBy(o => o.FriendlyChannelSub)
                    .ThenBy(o => o.FriendlyChannelNumber)
                    .ToList();
            }
        }
    }
}
