using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    class Program
    {


        private static CombinedSmhrData AllSmhrData { get; set; }
        private static TBLData AllTBLData { get; set; }
        private static PRMData AllPRMData { get; set; }
        private static DatabaseData AllDatabaseData { get; set; }
        private static List<string> FailedFiles { get; set; }

        //https://www.connectionstrings.com/mysql-connector-odbc-5-1/
        //https://www.connectionstrings.com/mysql-connector-odbc-5-1/info-and-download/
        //private static string ConnectionString { get; set; } = "server=localhost;user=root;database=mydb;port=3306;password=8CatJumpropeBazooka!";
        private static string ConnectionString { get; set; } = "server=10.63.4.107;user=dummy;database=mydb;port=3306;password=Passw0rd";
        public enum EFileType
        {
            smhr,
            tbl, 
            prm,
            database,
            directory,
            NAN
        };

        private static bool IsDirectory { get; set; }
        //private static EFileType InputFileType { get; set; }

        static void Main(string[] args)
        {
            bool quitflag = false;
            EFileType fileType;
            while (!quitflag)
            {
                bool success = ReadInput(out fileType, out quitflag);

                if (success)
                {
                    bool postProcessorsuccess = RunPostProcessor(fileType, out quitflag);

                    if (postProcessorsuccess)
                    {
                        System.Windows.Forms.MessageBox.Show("Successfully produced output file.");
                    }
                }
            }
        }

        private static bool ReadInput(out EFileType fileType, out bool quitflag)
        {
            quitflag = false;
            string filePath;
            System.IO.StreamReader file;
            bool success;
            while (true)
            {
                Console.WriteLine("Please enter the complete file path to the file or directory you would like to parse. Enter 'Q' to quit: ");

                filePath = Console.ReadLine();

                try
                {
                    if (filePath.ToUpper().Equals("Q"))
                    {
                        quitflag = true;
                        fileType = EFileType.NAN;
                        return false;
                    }
                    else
                    {
                        System.IO.FileAttributes attr = System.IO.File.GetAttributes(filePath);

                        if (attr.HasFlag(System.IO.FileAttributes.Directory))
                        {
                            FailedFiles = new List<string>();
                            fileType = EFileType.directory;
                            IsDirectory = true;
                            success = ReadDirectory(filePath);
                            break;
                        }
                        else
                        {
                            file = new System.IO.StreamReader(filePath);
                            IsDirectory = false;
                            success = ReadFile(filePath, file, out fileType);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    quitflag = false;
                    fileType = EFileType.NAN;
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

            return success;
        }

        private static bool ReadDirectory(string directoryPath)
        {
            string[] allFilePaths = System.IO.Directory.GetFiles(directoryPath);

            System.IO.StreamReader file;

            bool success;
            bool postProcessorSuccess;
            foreach (string s in allFilePaths)
            {
                if (RecognizedFileType(s))
                {
                    file = new System.IO.StreamReader(s);
                    success = ReadFile(s, file, out EFileType fileType);
                    if (!success)
                    {
                        FailedFiles.Add(s);
                    }
                    else
                    {

                        string outputName = ""; 
                        int periodIndex = GetLastIndexOf(s, ".");
                        if (periodIndex != -1)
                        {
                            outputName = s.Substring(0, periodIndex);
                        }
                        postProcessorSuccess = PostProcessFile(fileType, outputName + "csv");

                        if (!postProcessorSuccess)
                        {
                            FailedFiles.Add(s);
                        }
                    }
                }
            }

            return true;
        }

        private static int GetLastIndexOf(string s, string lookForString)
        {
            int lastIndexOf = -1;
            List<int> allInts = new List<int>();
            while (s.Contains(lookForString))
            {
                lastIndexOf = s.IndexOf(lookForString);
                allInts.Add(lastIndexOf + 1);
                s = s.Substring(lastIndexOf + 1);
            }

            int counter = 0;
            foreach (int i in allInts)
            {
                counter += i;
            }

            return counter;
        }

        private static bool RecognizedFileType(string path)
        {
            if (path.Contains(".prm") || path.Contains(".tbl") || path.Contains(".smhr") || (!path.Contains(".")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static bool ReadFile(string filePath, System.IO.StreamReader file, out EFileType fileType)
        {
            bool success;
            if (filePath.Contains(".smhr"))
            {
                AllSmhrData = new CombinedSmhrData();
                fileType = EFileType.smhr;
                success = AllSmhrData.ReadSMHRFile(filePath, file);
            }
            else if (filePath.Contains(".tbl"))
            {
                AllTBLData = new TBLData();
                fileType = EFileType.tbl;
                success = AllTBLData.ReadTBLFile(filePath, file);
            }
            else if (filePath.Contains(".prm"))
            {
                AllPRMData = new PRMData();
                fileType = EFileType.prm;
                success = AllPRMData.ReadPRMFile(filePath, file);
            }
            else if (!filePath.Contains("."))
            {
                AllDatabaseData = new DatabaseData();
                fileType = EFileType.database;
                success = AllDatabaseData.ReadDatabaseFile(file);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Input filetype not recognized.");
                fileType = EFileType.NAN;
                success = false;
            }

            return success;
        }

        private static bool RunPostProcessor(EFileType fileType, out bool quitflag)
        {
            quitflag = false;
            return PostProcessFile(fileType, ConnectionString);

            /*
            string writeFilePath = "";
            quitflag = false;
            if (IsDirectory)
            {
                //We don't want to have multiple dialog boxes popping up if we're looking at a directory. 
                return PostProcessFile(EFileType.directory, "");
            }
            else if (fileType != EFileType.database) //If the filetype is a database, we already know where to write the file.
            {
                while (true)
                {
                    Console.WriteLine("Please enter the complete file path to the output data file. Enter 'Q' to quit: ");

                    try
                    {
                        writeFilePath = Console.ReadLine();

                        if (writeFilePath.ToUpper().Equals("Q"))
                        {
                            quitflag = true;
                            return false;
                        }
                        else if (!writeFilePath.Contains("\\"))
                        {
                            Console.WriteLine("You did not specify a filepath. Are you sure you want to continue? (Y/N): ");
                            string yesNo = Console.ReadLine();

                            if (yesNo.ToUpper().Contains("Y"))
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.Message);
                        return false;
                    }
                }
            }

            return PostProcessFile(fileType, writeFilePath);
            */
        }

        private static bool PostProcessFile(EFileType fileType, string connectionString = "")
        {
            bool success;
            switch (fileType)
            {
                case EFileType.database:
                    success = AllDatabaseData.PostProcessDatabase();
                    break;
                case EFileType.prm:
                    success = AllPRMData.PostProcessPRM(ConnectionString);
                    break;
                case EFileType.smhr:
                    success = AllSmhrData.PostProcessSmhr(ConnectionString);
                    break;
                case EFileType.tbl:
                    success = AllTBLData.PostProcessTBL(ConnectionString);
                    break;
                case EFileType.directory:
                    success = PostProcessDirectory();
                    break;
                default:
                    success = false;
                    break;
            }
            return success;
        }

        private static bool PostProcessDirectory()
        {
            if (FailedFiles.Count == 0)
            {
                return true;
            }
            else
            {

                string combinedMessage = "Was unable to produce output files for:\n";
                foreach (string s in FailedFiles)
                {
                    combinedMessage += s + "\n";
                }

                System.Windows.Forms.MessageBox.Show(combinedMessage);
                return false;
            }
        }
    }
}

