using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    public class PRMData : ParsingHelperClass
    {
        public string FileName { get; set; }
        public string Classification { get; set; }
        public string ADPName { get; set; }
        public string ObjectName { get; set; }
        //public string WavebandName { get; set; }

        public PRMData()
        { }

        public bool ReadPRMFile(string filePath, System.IO.StreamReader file)
        {
            bool success;

            FileName = GetFileNameFromPath(filePath);
            try
            {
                string lineOne = file.ReadLine();

                string[] lineOneSplit = lineOne.Split(' ');
                int temp = 0;
                ReadNextNonEmptyString(lineOneSplit, temp, out string _, out temp, "%");
                ReadNextNonEmptyString(lineOneSplit, temp, out string classification, out temp, "classification");
                Classification = classification;

                string lineTwo = file.ReadLine();

                string[] lineTwoSplit = lineTwo.Split(' ');
                temp = 0;
                ReadNextNonEmptyString(lineOneSplit, temp, out string _, out temp, "%");
                ReadNextNonEmptyString(lineTwoSplit, temp, out string adpName, out temp, "adpName");
                ReadNextNonEmptyString(lineTwoSplit, temp, out string objName, out temp, "objName");
                ADPName = adpName;
                ObjectName = objName;

                file.Close();
                success = true;
            }
            catch (Exception e)
            {
                file.Close();
                System.Windows.Forms.MessageBox.Show("Error in reading prm file: " + e);
                success = false;
            }

            return success;
        }

        public bool PostProcessPRM(string filePath) 
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.WriteLine("Filename," + FileName);
                    file.WriteLine("Classification," + Classification);
                    file.WriteLine("ADP_Name," + ADPName);
                    file.WriteLine("Object_Name," + ObjectName);
                }

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error in writing to file:\n" + e.Message);
                return false;
            }
        }


    }
}
