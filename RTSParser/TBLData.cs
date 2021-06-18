using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    class TBLData : ParsingHelperClass
    {
        public string FileName { get; set; }
        public string Classification { get; set; }
        public string ADPName { get; set; }
        public string ObjectName { get; set; }
        public string FrequencyBand { get; set; }
        public string TblTableName { get; set; }

        public TBLData()
        {

        }

        public bool ReadTBLFile(string filePath, System.IO.StreamReader file)
        {
            bool success;

            //Already know this should work because otherwise would have quit. 
            //System.IO.StreamReader file = new System.IO.StreamReader(filePath);

            FileName = GetFileNameFromPath(filePath);
            try
            {
                string lineOne = file.ReadLine();

                string[] lineOneSplit = lineOne.Split(' ');
                int temp = 0;
                ReadNextNonEmptyString(lineOneSplit, temp, out string _, out temp, "#");
                ReadNextNonEmptyString(lineOneSplit, temp, out string classification, out temp, "classification");
                Classification = classification;

                string lineTwo = file.ReadLine();

                string[] lineTwoSplit = lineTwo.Split(' ');
                temp = 0;
                ReadNextNonEmptyString(lineOneSplit, temp, out string _, out temp, "#");
                ReadNextNonEmptyString(lineTwoSplit, temp, out string adpName, out temp, "adpName");
                ReadNextNonEmptyString(lineTwoSplit, temp, out string objName, out temp, "objName");
                ReadNextNonEmptyString(lineTwoSplit, temp, out string freqBand, out temp, "freqBand");
                ADPName = adpName;
                ObjectName = objName;
                FrequencyBand = freqBand;

                success = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error in reading tbl file: " + e);
                success = false;
            }

            file.Close();
            return success;
        }

        public bool PostProcessTBL(string connectionString)
        {
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                conn.Close();
                return false;
            }

            try
            {
                conn.Open();

                //string sql = "INSERT INTO Country (Code, Name, HeadOfState, Continent) VALUES ('ZYZ','DisneylandTwo','Mickey Mouse', 'North America')";
                //string sql = "INSERT INTO tbssignatureofweapon (ID, Type, FileLocation, ID_Weapon) VALUES (1,'sdoifj','osiefjs',12)";
                //string sql = "INSERT INTO tbrobjecttype_tgx (ID, ID_UID, ID_Object, Type) VALUES (12, 'soidjf', 'sdoifjse', 'woeifjoies')";
                //string sql = "INSERT INTO tblweapon VALUES (12, 'osidjf', 'sdfasdf', 'sdfefse', 'qw3eqweqw', '32rfefw', 23, 89, 54)";
                string sql = "SELECT MAX(ID) FROM ___";
                //FlightStages
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                int nextIndex = -1;
                while (rdr.Read())
                {
                    try
                    {
                        Console.WriteLine(rdr[0]);
                        nextIndex = Int32.Parse(rdr[0].ToString()) + 1;
                        //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        nextIndex = 1;
                    }
                }

                rdr.Close();
                //FlightStages - all entries can be null
                string mainCommand = "";
                MySqlCommand cmdMain = new MySqlCommand(mainCommand, conn);
                MySqlDataReader rdrMain = cmdMain.ExecuteReader();

                while (rdrMain.Read())
                {
                    Console.WriteLine(rdr[0]);
                    //nextIndex = Int32.Parse(rdr[0].ToString());
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }

                rdrMain.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                conn.Close();
                return false;
            }

            conn.Close();
            Console.WriteLine("Done.");

            return true;


            /*
        int numAffectedRows = 0;
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                //"INSERT INTO _ (_,_,...) values('_','_',...)"
                string command = "INSERT INTO " + TblTableName + " (";
                numAffectedRows += RunODBCCommand(command, connection);
            }

            if (numAffectedRows == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            */

            /*
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.WriteLine("Filename," + FileName);
                    file.WriteLine("Classification," + Classification);
                    file.WriteLine("ADP_Name," + ADPName);
                    file.WriteLine("Object_Name," + ObjectName);
                    file.WriteLine("Frequency_Band, " + FrequencyBand);
                }

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error in writing to file:\n" + e.Message);
                return false;
            }
            */
        }
    }
}
