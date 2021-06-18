using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RTSParser
{
    class DatabaseData : ParsingHelperClass
    {
        private struct Entry {
            public string missile_db_id;
            public string adp_name;
            public string msl_sysid;
        };

        private List<Entry> AllEntries { get; set; }

        public DatabaseData()
        {
            AllEntries = new List<Entry>();
        }


        public bool ReadDatabaseFile(string filePath, System.IO.StreamReader file)
        {
            bool success;

            try
            { 
                string currentLine = "";
                //ReadLine returns null when the streamreader reaches the end of the file.
                while (currentLine != null)
                {
                    if (currentLine.Equals("[missile_entry_start]"))
                    {
                        Entry entry = new Entry();

                        while (!currentLine.Equals("[missile_entry_end]"))
                        {
                            currentLine = file.ReadLine();

                            if (currentLine.Contains("missile_db_id"))
                            {
                                entry.missile_db_id = GetValueFromKeyEqValueFormat(currentLine);
                            }
                            else if (currentLine.Contains("adp_name"))
                            {
                                entry.adp_name = GetValueFromKeyEqValueFormat(currentLine);
                            }
                            else if (currentLine.Contains("msl_sysid"))
                            {
                                entry.msl_sysid = GetValueFromKeyEqValueFormat(currentLine);
                            }
                        }

                        AllEntries.Add(entry);
                    }

                    currentLine = file.ReadLine();
                }

                file.Close();
                success = true;
            }
            catch (Exception e)
            {
                file.Close();
                System.Windows.Forms.MessageBox.Show("Error in reading database file:\n" + e);
                success = false;
            }

            return success;
        }

        public bool PostProcessDatabase()
        {
            try
            {
                string localPath = Environment.GetEnvironmentVariable("LocalAppData");
                string databaseFilePath = localPath + "\\MAFTL\\ParserDatabaseFile";
                string fileName = "\\ProcessedDatabaseFile.xml";
                System.IO.Directory.CreateDirectory(databaseFilePath);
                //string databaseFilePath = "..\\..\\DatabaseFile\\ProcessedDatabaseFile.xml";

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding("utf-8");
                XmlWriter xmlWriter = XmlWriter.Create(databaseFilePath + fileName, settings);

                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("Entries");

                foreach (Entry entry in AllEntries)
                {
                    xmlWriter.WriteStartElement("Entry");
                    xmlWriter.WriteElementString("missile_db_id", entry.missile_db_id);
                    xmlWriter.WriteElementString("adp_name", entry.adp_name);
                    xmlWriter.WriteElementString("msl_sysid", entry.msl_sysid);

                    //Closes Entry
                    xmlWriter.WriteEndElement();
                }

                //closes Entries
                xmlWriter.WriteEndElement();

                xmlWriter.Close();

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error in writing to file:\n" + e.Message);
                return false;
            }
        }

        private string GetValueFromKeyEqValueFormat(string inputString)
        {
            int equalSignIndex = inputString.IndexOf('=');
            return inputString.Substring(equalSignIndex + 2);
        }

    }
}
