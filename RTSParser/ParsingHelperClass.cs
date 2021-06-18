using System;
using System.Collections.Generic;
using System.Data.Odbc;
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

        public static int RunODBCCommand(string commandString, System.Data.Odbc.OdbcConnection connection)
        {
            OdbcCommand command = new OdbcCommand(commandString,connection);
            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}
