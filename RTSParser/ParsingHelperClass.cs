using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    public abstract class ParsingHelperClass
    {
        public static string ReadNextValToArray(string[] overallString, int startIndex, string nameOfParameter, out int endIndex)
        {
            ReadNextNonEmptyString(overallString, startIndex, out string temp, out endIndex, nameOfParameter);
            return temp;
        }

        public static void ReadNextNonEmptyString(string[] overallString, int startIndex, out string nextNonEmptyString, out int endIndex, string nameOfParameter)
        {
            try
            {
                nextNonEmptyString = "";
                endIndex = -1;

                for (int i = startIndex; i < overallString.Length; i++)
                {
                    if (overallString[i] != "")
                    {
                        nextNonEmptyString = overallString[i];
                        endIndex = i + 1;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error reading file. Parameter: " + nameOfParameter + " is not found.");
            }
        }

        public static string GetFileNameFromPath(string filePath)
        {
            if (filePath != "")
            {
                while (filePath.Contains("\\"))
                {
                    filePath = filePath.Substring(filePath.IndexOf("\\") + 1);
                }
            }

            return filePath;
        }

        public static int GetLowestUnusedID(MySqlConnection conn, string tableName)
        {
            int id = -1;
            try
            {

                //First need to get the primary key: 
                string command = "Show KEYS FROM " + tableName + " WHERE Key_name = 'PRIMARY'";
                MySqlCommand cmd = new MySqlCommand(command, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                string primaryKeyColumnName = "";
                while (rdr.Read())
                {
                    //The fourth index is always going to be the column name of the primary key. 
                    Console.WriteLine(rdr[4]);
                    primaryKeyColumnName = rdr[4].ToString();
                }
                //rdr.Close();

                rdr.Close();



                string maxCmdStr = "SELECT MAX(" + primaryKeyColumnName + ") FROM " + tableName;
                //FlightStages
                MySqlCommand maxCmd = new MySqlCommand(maxCmdStr, conn);
                MySqlDataReader maxRdr = maxCmd.ExecuteReader();

                while (maxRdr.Read())
                {
                    try
                    {
                        Console.WriteLine(maxRdr[0]);
                        id = Int32.Parse(maxRdr[0].ToString()) + 1;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        id = 1;
                    }
                    catch (FormatException)
                    {
                        id = 1;
                    }
                }
                maxRdr.Close();

                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                conn.Close();
                return -1;
            }
        }

        public static long GetFileSizeFromPath(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                return fi.Length;
            }
            return -1;
        }

        //public static void RunSQLCommand(string commandString, MySqlConnection connection)
        //{
        //    MySqlCommand cmdFlightStages = new MySqlCommand(commandString, connection);
        //
        //    /*
        //    OdbcCommand command = new OdbcCommand(commandString,connection);
        //    connection.Open();
        //    return command.ExecuteNonQuery();
        //    */
        //}
    }
}
