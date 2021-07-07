using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RTSParser
{

    class CombinedSmhrData : ParsingHelperClass
    {
        private List<SmhrDynamicData> AllDynamicData { get; set; }
        private SmhrStaticData AllStaticData { get; set; }
        private List<int> tblplatformIDs { get; set; }
        private List<int> tblobjectIDs { get; set; }

        public CombinedSmhrData()
        {

        }

        public bool ReadSMHRFile(string filePath, System.IO.StreamReader file)
        {
            //Already know this should work because otherwise would have quit. 
            //System.IO.StreamReader file = new System.IO.StreamReader(filePath);

            AllStaticData = new SmhrStaticData();
            AllDynamicData = new List<SmhrDynamicData>();

            //!!!THIS MAY NEED TO CHANGE!!!
            AllStaticData.FileLocation = filePath;
            AllStaticData.Filename = GetFileNameFromPath(filePath);
            AllStaticData.FileSize = GetFileSizeFromPath(filePath);
            try
            {

                string line;

                //Often begins with commented out lines, beginning with #
                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Contains("#"))
                    {
                        break;
                    }
                }

                string[] recordZero = line.Split(' ');

                string smhrVersionNumber = "";
                int temp = 0;
                for (int i = 0; i < recordZero.Length; i++)
                {
                    smhrVersionNumber += recordZero[i] + " ";

                    if (recordZero[i].Contains("v"))
                    {
                        temp = i;
                        break;
                    }
                }
                AllStaticData.SMHRVersionNumber = smhrVersionNumber;

                string applicationUsed = "";
                if (temp == recordZero.Length)
                {
                    throw new Exception("Unable to find smhr Version number");
                }
                for (int i = temp + 1; i < recordZero.Length; i++)
                {
                    applicationUsed += recordZero[i] + " ";
                }
                AllStaticData.ApplicationUsed = applicationUsed;

                string[] recordOne = file.ReadLine().Split(' ');
                string classification = recordOne[0];
                AllStaticData.Classification = classification;

                string[] recordTwo = file.ReadLine().Split(' ');

                string threatName = "";
                for (int i = 0; i < recordTwo.Length; i++)
                {
                    if (recordTwo[i].Contains("/"))
                    {
                        temp = i;
                        break;
                    }

                    threatName += recordTwo[i] + " ";
                }
                AllStaticData.ThreatName = threatName;

                string date = recordTwo[temp];
                AllStaticData.Date = date;

                //Read record Three
                string recordThree = file.ReadLine();
                string tmssIRSVersionNumber = recordThree;
                AllStaticData.TmssIRSVersionNumber = tmssIRSVersionNumber;

                //Read record Four
                string[] recordFour = file.ReadLine().Split(' ');
                temp = 0;
                ReadNextNonEmptyString(recordFour, temp, out string earthGravityModel, out temp, "earthGravityModel");
                ReadNextNonEmptyString(recordFour, temp, out string earthGravityModelComponentN, out temp, "earthGravityModelComponentN");
                ReadNextNonEmptyString(recordFour, temp, out string earthGravityModelComponentM, out temp, "earthGravityModelComponentM");
                ReadNextNonEmptyString(recordFour, temp, out string atmosphericModel, out temp, "atmosphericModel");
                ReadNextNonEmptyString(recordFour, temp, out string rotation, out temp, "rotation");
                ReadNextNonEmptyString(recordFour, temp, out string rotationCoordinateFrame, out temp, "rotationCoordinateFrame");
                ReadNextNonEmptyString(recordFour, temp, out string reentryAltitude, out temp, "reentryAltitude");

                AllStaticData.EarthGravityModel = earthGravityModel;
                AllStaticData.EarthGravityModelComponentN = earthGravityModelComponentN;
                AllStaticData.EarthGravityModelComponentM = earthGravityModelComponentM;
                AllStaticData.AtmosphericModel = atmosphericModel;
                AllStaticData.Rotation = rotation;
                AllStaticData.RotationCoordinateFrame = rotationCoordinateFrame;
                AllStaticData.ReentryAltitude = reentryAltitude;


                string epochTime;
                if (temp == recordFour.Length)
                {
                    epochTime = "";
                }
                else
                {
                    ReadNextNonEmptyString(recordFour, temp, out epochTime, out temp, "epochTime");
                }

                AllStaticData.EpochTime = epochTime;

                //Read record Five
                string[] recordFive = file.ReadLine().Split(' ');
                temp = 0;
                ReadNextNonEmptyString(recordFive, temp, out string missileID, out temp, "missileID");
                ReadNextNonEmptyString(recordFive, temp, out string systemName, out temp, "systemName");
                ReadNextNonEmptyString(recordFive, temp, out string systemType, out temp, "systemType");
                ReadNextNonEmptyString(recordFive, temp, out string systemOperation, out temp, "systemOperation");
                ReadNextNonEmptyString(recordFive, temp, out string numberPBVsOnMissile, out temp, "numberPBVsOnMissile");
                ReadNextNonEmptyString(recordFive, temp, out string numberRVsOnMissile, out temp, "numberRVsOnMissile");
                ReadNextNonEmptyString(recordFive, temp, out string numberObjectsOnMissile, out temp, "numberObjectsOnMissile");
                ReadNextNonEmptyString(recordFive, temp, out string bboInertialRange, out temp, "bboInertialRange");
                ReadNextNonEmptyString(recordFive, temp, out string bboGroundRange, out temp, "bboGroundRange");
                ReadNextNonEmptyString(recordFive, temp, out string bboAltitude, out temp, "bboAltitude");
                ReadNextNonEmptyString(recordFive, temp, out string bboFlightPathAngle, out temp, "bboFlightPathAngle");
                ReadNextNonEmptyString(recordFive, temp, out string bboAzimuth, out temp, "bboAzimuth");

                AllStaticData.MissileID = missileID;
                AllStaticData.SystemName = systemName;
                AllStaticData.SystemType = systemType;
                AllStaticData.SystemOperation = systemOperation;
                AllStaticData.NumberPBVsOnMissile = numberPBVsOnMissile;
                AllStaticData.NumberRVsOnMissile = numberRVsOnMissile;
                AllStaticData.NumberObjectsOnMissile = numberObjectsOnMissile;
                AllStaticData.BboInertialRange = bboInertialRange;
                AllStaticData.BboGroundRange = bboGroundRange;
                AllStaticData.BboAltitude = bboAltitude;
                AllStaticData.BboFlightPathAngle = bboFlightPathAngle;
                AllStaticData.BboAzimuth = bboAzimuth;

                //Read record Six
                string[] recordSix = file.ReadLine().Split(' ');
                temp = 0;
                ReadNextNonEmptyString(recordSix, temp, out string bboTime, out temp, "bboTime");
                ReadNextNonEmptyString(recordSix, temp, out string liftOffTime, out temp, "liftOffTime");
                ReadNextNonEmptyString(recordSix, temp, out string launchComplex, out temp, "launchComplex");
                ReadNextNonEmptyString(recordSix, temp, out string launchGeodeticLatitude, out temp, "launchGeodeticLatitude");
                ReadNextNonEmptyString(recordSix, temp, out string launchGeodeticLongitude, out temp, "launchGeodeticLongitude");
                ReadNextNonEmptyString(recordSix, temp, out string launchAltitude, out temp, "launchAltitude");
                ReadNextNonEmptyString(recordSix, temp, out string launchTime, out temp, "launchTime");
                ReadNextNonEmptyString(recordSix, temp, out string launchAzimuth, out temp, "launchAzimuth");
                ReadNextNonEmptyString(recordSix, temp, out string numberOfObjectSetsToFollow, out temp, "numberOfObjectSetsToFollow");
                ReadNextNonEmptyString(recordSix, temp, out string uniqueAllocationId, out temp, "uniqueAllocationId");
                ReadNextNonEmptyString(recordSix, temp, out string randomSeed, out temp, "randomSeed");

                AllStaticData.BboTime = bboTime;
                AllStaticData.LiftOffTime = liftOffTime;
                AllStaticData.LaunchComplex = launchComplex;
                AllStaticData.LaunchGeodeticLatitude = launchGeodeticLatitude;
                AllStaticData.LaunchGeodeticLongitude = launchGeodeticLongitude;
                AllStaticData.LaunchAltitude = launchAltitude;
                AllStaticData.LaunchTime = launchTime;
                AllStaticData.LaunchAzimuth = launchAzimuth;
                AllStaticData.NumberOfObjectSetsToFollow = numberOfObjectSetsToFollow;
                AllStaticData.UniqueAllocationId = uniqueAllocationId;
                AllStaticData.RandomSeed = randomSeed;


                string[] recordSeven;
                string[] recordTen = null;
                bool newObjectflag = false;
                do
                {
                    SmhrDynamicData dynamicData = new SmhrDynamicData();
                    if (newObjectflag)
                    {
                        recordSeven = recordTen;
                        newObjectflag = false;
                    }
                    else
                    {
                        recordSeven = file.ReadLine().Split(' ');
                    }

                    //Parse record Seven
                    temp = 0;
                    ReadNextNonEmptyString(recordSeven, temp, out string objectName, out temp, "objectName");
                    ReadNextNonEmptyString(recordSeven, temp, out string objectType, out temp, "objectType");
                    ReadNextNonEmptyString(recordSeven, temp, out string pbvAssociationNumber, out temp, "pbvAssociationNumber");
                    ReadNextNonEmptyString(recordSeven, temp, out string objectNumber, out temp, "objectNumber");
                    ReadNextNonEmptyString(recordSeven, temp, out string objectCategory, out temp, "objectCategory");
                    ReadNextNonEmptyString(recordSeven, temp, out string criticalPathObject, out temp, "criticalPathObject");
                    ReadNextNonEmptyString(recordSeven, temp, out string parentObjectNumber, out temp, "parentObjectNumber");
                    ReadNextNonEmptyString(recordSeven, temp, out string parentXVelocity, out temp, "parentXVelocity");
                    ReadNextNonEmptyString(recordSeven, temp, out string parentYVelocity, out temp, "parentYVelocity");
                    ReadNextNonEmptyString(recordSeven, temp, out string parentZVelocity, out temp, "parentZVelocity");
                    ReadNextNonEmptyString(recordSeven, temp, out string ballisticCoefficientReferenceSource, out temp, "ballisticCoefficientReferenceSource");
                    ReadNextNonEmptyString(recordSeven, temp, out string objectTypeDeploymentSequenceNumber, out temp, "objectTypeDeploymentSequenceNumber");

                    dynamicData.ObjectName = objectName;
                    dynamicData.ObjectType = objectType;
                    dynamicData.PbvAssociationNumber = pbvAssociationNumber;
                    dynamicData.ObjectNumber = objectNumber;
                    dynamicData.ObjectCategory = objectCategory;
                    dynamicData.CriticalPathObject = criticalPathObject;
                    dynamicData.ParentObjectNumber = parentObjectNumber;
                    dynamicData.ParentXVelocity = parentXVelocity;
                    dynamicData.ParentYVelocity = parentYVelocity;
                    dynamicData.ParentZVelocity = parentZVelocity;
                    dynamicData.BallisticCoefficientReferenceSource = ballisticCoefficientReferenceSource;
                    dynamicData.ObjectTypeDeploymentSequenceNumber = objectTypeDeploymentSequenceNumber;

                    //Read record Eight
                    string[] recordEight = file.ReadLine().Split(' ');
                    temp = 0;
                    ReadNextNonEmptyString(recordEight, temp, out string objectDestructionGeodeticLat, out temp, "objectDestructionGeodeticLat");
                    ReadNextNonEmptyString(recordEight, temp, out string objectDestructionGeodeticLon, out temp, "objectDestructionGeodeticLon");
                    ReadNextNonEmptyString(recordEight, temp, out string objectDestructionAlt, out temp, "objectDestructionAlt");
                    ReadNextNonEmptyString(recordEight, temp, out string objectDestructionTime, out temp, "objectDestructionTime");
                    ReadNextNonEmptyString(recordEight, temp, out string objectReentryAngle, out temp, "objectReentryAngle");
                    ReadNextNonEmptyString(recordEight, temp, out string reentryTime, out temp, "reentryTime");
                    ReadNextNonEmptyString(recordEight, temp, out string apogeeAltitude, out temp, "apogeeAltitude");
                    ReadNextNonEmptyString(recordEight, temp, out string apogeeTime, out temp, "apogeeTime");

                    dynamicData.ObjectDestructionGeodeticLat = objectDestructionGeodeticLat;
                    dynamicData.ObjectDestructionGeodeticLon = objectDestructionGeodeticLon;
                    dynamicData.ObjectDestructionAlt = objectDestructionAlt;
                    dynamicData.ObjectDestructionTime = objectDestructionTime;
                    dynamicData.ObjectReentryAngle = objectReentryAngle;
                    dynamicData.ReentryTime = reentryTime;
                    dynamicData.ApogeeAltitude = apogeeAltitude;
                    dynamicData.ApogeeTime = apogeeTime;

                    //Read record Nine
                    string[] recordNine = file.ReadLine().Split(' ');
                    temp = 0;
                    ReadNextNonEmptyString(recordNine, temp, out string aimpointId, out temp, "aimpointId");
                    ReadNextNonEmptyString(recordNine, temp, out string aimpointGeodeticLatitude, out temp, "aimpointGeodeticLatitude");
                    ReadNextNonEmptyString(recordNine, temp, out string aimpointGeodeticLongitude, out temp, "aimpointGeodeticLongitude");
                    ReadNextNonEmptyString(recordNine, temp, out string aimpointAltitude, out temp, "aimpointAltitude");
                    ReadNextNonEmptyString(recordNine, temp, out string numberOfStatesToFollow, out temp, "numberOfStatesToFollow");

                    dynamicData.AimpointId = aimpointId;
                    dynamicData.AimpointGeodeticLatitude = aimpointGeodeticLatitude;
                    dynamicData.AimpointGeodeticLongitude = aimpointGeodeticLongitude;
                    dynamicData.AimpointAltitude = aimpointAltitude;
                    dynamicData.NumberOfStatesToFollow = numberOfObjectSetsToFollow;

                    recordTen = file.ReadLine().Split(' ');
                    while (!recordTen[0].Contains('#'))
                    {
                        //Record10
                        temp = 0;
                        dynamicData.Time.Add(ReadNextValToArray(recordTen, temp, "time", out temp));
                        dynamicData.RVAssociationNumber.Add(ReadNextValToArray(recordTen, temp, "RVAssociationNumber", out temp));
                        dynamicData.OnCriticalPathFlag.Add(ReadNextValToArray(recordTen, temp, "OnCriticalPathFlag", out temp));
                        dynamicData.EventStatusFlag.Add(ReadNextValToArray(recordTen, temp, "EventStatusFlag", out temp));
                        dynamicData.EngineStatusFlag.Add(ReadNextValToArray(recordTen, temp, "EngineStatusFlag", out temp));
                        dynamicData.GuidanceDirectionFlag.Add(ReadNextValToArray(recordTen, temp, "guidanceDirectionFlag", out temp));
                        dynamicData.GuidanceManeuverMode.Add(ReadNextValToArray(recordTen, temp, "GuidanceManeuverMode", out temp));
                        dynamicData.FuelFlowRate.Add(ReadNextValToArray(recordTen, temp, "FuelFlowRate", out temp));
                        dynamicData.BallisticCoefficient.Add(ReadNextValToArray(recordTen, temp, "ballisticCoefficient", out temp));
                        dynamicData.AerodynamicMode.Add(ReadNextValToArray(recordTen, temp, "AerodynamicsMode", out temp));
                        dynamicData.XPosition.Add(ReadNextValToArray(recordTen, temp, "xPosition", out temp));
                        dynamicData.YPosition.Add(ReadNextValToArray(recordTen, temp, "yPosition", out temp));
                        dynamicData.ZPosition.Add(ReadNextValToArray(recordTen, temp, "zPosition", out temp));

                        //Record 11
                        string[] recordEleven = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.XVelocity.Add(ReadNextValToArray(recordEleven, temp, "XVelocity", out temp));
                        dynamicData.YVelocity.Add(ReadNextValToArray(recordEleven, temp, "YVelocity", out temp));
                        dynamicData.ZVelocity.Add(ReadNextValToArray(recordEleven, temp, "ZVelocity", out temp));
                        dynamicData.XAccelleration.Add(ReadNextValToArray(recordEleven, temp, "XAccelleration", out temp));
                        dynamicData.YAccelleration.Add(ReadNextValToArray(recordEleven, temp, "YAccelleration", out temp));
                        dynamicData.ZAccelleration.Add(ReadNextValToArray(recordEleven, temp, "ZAccelleration", out temp));
                        dynamicData.LateralAccelleration.Add(ReadNextValToArray(recordEleven, temp, "LateralAccelleration", out temp));
                        dynamicData.AxialAccelleration.Add(ReadNextValToArray(recordEleven, temp, "AxialAccelleration", out temp));
                        dynamicData.YBodyFrameAccelleration.Add(ReadNextValToArray(recordEleven, temp, "YBodyFrameAccelleration", out temp));
                        dynamicData.ZBodyFrameAccelleration.Add(ReadNextValToArray(recordEleven, temp, "ZBodyFrameAccelleration", out temp));

                        //Record 12
                        string[] recordTwelve = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.XBodyAxis.Add(ReadNextValToArray(recordTwelve, temp, "XBodyAxis", out temp));
                        dynamicData.YBodyAxis.Add(ReadNextValToArray(recordTwelve, temp, "YBodyAxis", out temp));
                        dynamicData.ZBodyAxis.Add(ReadNextValToArray(recordTwelve, temp, "ZBodyAxis", out temp));
                        dynamicData.OrientationMode.Add(ReadNextValToArray(recordTwelve, temp, "OrientationMode", out temp));

                        //Record 13
                        string[] recordThirteen = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.XAxisEulerAnglePhi.Add(ReadNextValToArray(recordThirteen, temp, "XAxisEulerAnglePhi", out temp));
                        dynamicData.YAxisEulerAngleTheta.Add(ReadNextValToArray(recordThirteen, temp, "YAxisEulerAngleTheta", out temp));
                        dynamicData.ZAxisEulerAnglePsi.Add(ReadNextValToArray(recordThirteen, temp, "ZAxisEulerAnglePsi", out temp));
                        dynamicData.BodyXRollAxisAngularRate.Add(ReadNextValToArray(recordThirteen, temp, "BodyXRollAxisAngularRate", out temp));
                        dynamicData.BodyYPitchAxisAngularRate.Add(ReadNextValToArray(recordThirteen, temp, "BodyYPitchAxisAngularRate", out temp));
                        dynamicData.BodyZYawAxisAngularRate.Add(ReadNextValToArray(recordThirteen, temp, "BodyZYawAxisAngularRate", out temp));
                        dynamicData.RollMomentOfInteriaIxx.Add(ReadNextValToArray(recordThirteen, temp, "RollMomentOfInteriaIxx", out temp));
                        dynamicData.PitchMomentOfInertiaIyy.Add(ReadNextValToArray(recordThirteen, temp, "PitchMomentOfInertiaIyy", out temp));
                        dynamicData.YawMomentOfIntertiaIzz.Add(ReadNextValToArray(recordThirteen, temp, "YawMomentOfIntertiaIzz", out temp));
                        dynamicData.RollPitchProductOfInertiaIxy.Add(ReadNextValToArray(recordThirteen, temp, "RollPitchProductOfInertiaIxy", out temp));
                        dynamicData.RollYawProductOfInertiaIxz.Add(ReadNextValToArray(recordThirteen, temp, "RollYawProductOfInertiaIxz", out temp));
                        dynamicData.PitchYawProductOfInertiaIyz.Add(ReadNextValToArray(recordThirteen, temp, "PitchYawProductOfInertiaIyz", out temp));

                        //Record 14
                        string[] recordFourteen = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.QuaternionVectorXElementQ1.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionVectorXElementQ1", out temp));
                        dynamicData.QuaternionVectorYElementQ2.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionVectorYElementQ2", out temp));
                        dynamicData.QuaternionVectorZElementQ3.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionVectorZElementQ3", out temp));
                        dynamicData.QuaternionScalarElementQ4.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionScalarElementQ4", out temp));
                        dynamicData.QuaternionXElementQ1Rate.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionXElementQ1Rate", out temp));
                        dynamicData.QuaternionYElementQ2Rate.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionYElementQ2Rate", out temp));
                        dynamicData.QuaternionZElementQ3Rate.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionZElementQ3Rate", out temp));
                        dynamicData.QuaternionElementQ4Rate.Add(ReadNextValToArray(recordFourteen, temp, "QuaternionElementQ4Rate", out temp));

                        //Record 15
                        string[] recordFifteen = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.InertialFlightPathAngle.Add(ReadNextValToArray(recordFifteen, temp, "InertialFlightPathAngle", out temp));
                        dynamicData.PrecessionAngle.Add(ReadNextValToArray(recordFifteen, temp, "PrecessionAngle", out temp));
                        dynamicData.PrecessionRate.Add(ReadNextValToArray(recordFifteen, temp, "PrecessionRate", out temp));
                        dynamicData.SpinAngle.Add(ReadNextValToArray(recordFifteen, temp, "SpinAngle", out temp));
                        dynamicData.SpinRate.Add(ReadNextValToArray(recordFifteen, temp, "SpinRate", out temp));
                        dynamicData.CenterOfGravityLocationX.Add(ReadNextValToArray(recordFifteen, temp, "CenterOfGravityLocationX", out temp));
                        dynamicData.CenterOfGravityLocationY.Add(ReadNextValToArray(recordFifteen, temp, "CenterOfGravityLocationY", out temp));
                        dynamicData.CenterOfGravityLocationZ.Add(ReadNextValToArray(recordFifteen, temp, "CenterOfGravityLocationZ", out temp));
                        dynamicData.TotalAngleOfAttack.Add(ReadNextValToArray(recordFifteen, temp, "TotalAngleOfAttack", out temp));
                        dynamicData.PitchAngleOfAttack.Add(ReadNextValToArray(recordFifteen, temp, "PitchAngleOfAttack", out temp));
                        dynamicData.YawAngleOfAttack.Add(ReadNextValToArray(recordFifteen, temp, "YawAngleOfAttack", out temp));

                        //Record 16
                        string[] recordSixteen = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.Thrust.Add(ReadNextValToArray(recordSixteen, temp, "Thrust", out temp));
                        dynamicData.VacuumThrust.Add(ReadNextValToArray(recordSixteen, temp, "VacuumThrust", out temp));
                        dynamicData.ThrustMode.Add(ReadNextValToArray(recordSixteen, temp, "ThrustMode", out temp));
                        dynamicData.ThrustDirectionX.Add(ReadNextValToArray(recordSixteen, temp, "ThrustDirectionX", out temp));
                        dynamicData.ThrustDirectionY.Add(ReadNextValToArray(recordSixteen, temp, "ThrustDirectionY", out temp));
                        dynamicData.ThrustDirectionZ.Add(ReadNextValToArray(recordSixteen, temp, "ThrustDirectionZ", out temp));

                        //Record 17
                        string[] recordSeventeen = file.ReadLine().Split(' ');
                        temp = 0;
                        dynamicData.Mass.Add(ReadNextValToArray(recordSeventeen, temp, "Mass", out temp));
                        dynamicData.LongitudinalCenterOfPressurePitch.Add(ReadNextValToArray(recordSeventeen, temp, "LongitudinalCenterOfPressurePitch", out temp));
                        dynamicData.StaticStabilityIndicator.Add(ReadNextValToArray(recordSeventeen, temp, "StaticStabilityIndicator", out temp));
                        dynamicData.LongitudinalCenterOfPressureYaw.Add(ReadNextValToArray(recordSeventeen, temp, "LongitudinalCenterOfPressureYaw", out temp));
                        dynamicData.DynamicPressure.Add(ReadNextValToArray(recordSeventeen, temp, "DynamicPressure", out temp));
                        dynamicData.MachNumber.Add(ReadNextValToArray(recordSeventeen, temp, "MachNumber", out temp));
                        dynamicData.DragCoefficient.Add(ReadNextValToArray(recordSeventeen, temp, "DragCoefficient", out temp));

                        //After record 17, the smhr repeats back with record 10 at a new timestep. 
                        //This continues until the end of the object or the file.
                        //The end of the file is marked by a '#', while
                        //the end of the object will be seen by trying to parse the time to an integer. 
                        recordTen = file.ReadLine().Split(' ');
                        ReadNextNonEmptyString(recordTen, 0, out string tempStr, out int _, "Time");
                        bool alphaFlag = false;
                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            if (char.IsLetter(tempStr[i]))
                            {
                                alphaFlag = true;
                                break;
                            }
                        }

                        if (recordTen[0].Contains('#'))
                        {
                            //The '#' indicates we've arrived at the end of the file. 
                            AllDynamicData.Add(dynamicData);
                            recordSeven = recordTen; //To make sure the do while works. 
                        }
                        else if (alphaFlag)
                        {
                            //If the next character is a letter, we've arrived at a name, which means it's a new object. 
                            //This is a potential runtime bug - assumes that all names begin with a letter rather than a number.
                            //Not sure if this is enforced by the format, but this is the case for missile1.smhr. 
                            AllDynamicData.Add(dynamicData);
                            newObjectflag = true;
                            break;
                        }
                    }
                }
                while (!recordSeven[0].Contains('#'));

                //At this point, the file should have been read to the '#' character, which means
                //there is no more meaningful data. We should be good to close the file now.
                file.Close();

                //Indicates that the file was read successfully.
                return true;
            }
            catch (Exception e)
            {
                file.Close();
                System.Windows.Forms.MessageBox.Show(e.Message);

                //Indicates there was an error in reading the file. 
                return false;
            }
        }

        public bool PostProcessSmhr(string connectionString)
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
                

                string adp_name = GetADPNameFromSystemName(AllStaticData.SystemName);

                if (adp_name.Equals(""))
                {
                    throw new Exception("Unable to find adp_name for the missile type: " + AllStaticData.SystemName);
                }

                //Console.WriteLine("OSDIJF");
                conn.Open();


                //Now we have many tables. 
                //I have absolutely no idea if there's a smart way to map the database fields to the smhr fields dynamically. 
                //=> hard code :(. 
                return InputAllSmhrDataToDatabase(conn, adp_name);


                //Previous Implementation
                /*
                //string sql = "INSERT INTO Country (Code, Name, HeadOfState, Continent) VALUES ('ZYZ','DisneylandTwo','Mickey Mouse', 'North America')";
                //string sql = "INSERT INTO tbssignatureofweapon (ID, Type, FileLocation, ID_Weapon) VALUES (1,'sdoifj','osiefjs',12)";
                //string sql = "INSERT INTO tbrobjecttype_tgx (ID, ID_UID, ID_Object, Type) VALUES (12, 'soidjf', 'sdoifjse', 'woeifjoies')";
                //string sql = "INSERT INTO tblweapon VALUES (12, 'osidjf', 'sdfasdf', 'sdfefse', 'qw3eqweqw', '32rfefw', 23, 89, 54)";
                string sql = "SELECT MAX(ID) FROM MISSILE";
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
                    catch (FormatException)
                    {
                        nextIndex = 1;
                    }
                }

                rdr.Close();
                //FlightStages - all entries can be null
                string mainCommand = "INSERT INTO MISSILE (ID, " +
                    "Path, " +
                    "FileName, " +
                    "FileType, " +
                    "TrajectoryID, " +
                    "TrajectoryType, " + 
                    "ApplicationUsed, " +
                    "Classification, " +
                    "ScenarioName, " +
                    "Date, " +
                    "TMSSIRSVersionNumber, " +
                    "Specification, " +
                    "EarthGravityModel, " +
                    "FlightMode, " +
                    "EarthShapeModel, " + 
                    "AtmosphericModel, " +
                    "Rotation, " +
                    "RotationCoordinateFrame, " +
                    "EpochTime, " +
                    "SystemNameTMSS, " +
                    "SystemNameRTS, " +
                    "ADPName, " +
                    "SystemNameIntel, " + 
                    "NumberPBVsOnMissile, " +
                    "NumberRVsOnMissile, " +
                    "NumberObjectsOnMissile, " +
                    "BBOGroundRange, " +
                    "GroundRange, " +
                    "BBOAltitude, " +
                    "BBOTime, " +
                    "LaunchpointName, " +
                    "LaunchRegion, " +
                    "LaunchCountry, " +
                    "CountryOfOrigin, " +
                    "LaunchGeodeticLatitude, " +
                    "LaunchGeodeticLongitude, " +
                    "LaunchTime, " +
                    "LaunchAzimuth, " +
                    "ImpactTime, " +
                    "FlightTime, " +
                    "WarheadType, " +
                    "Countermeasures) VALUES (" +
                    nextIndex + ",'" +
                    AllStaticData.FileLocation + "','" +
                    AllStaticData.Filename + "','" +
                    AllStaticData.FileType + "'," + 
                    "NULL,'" + //TrajectoryID is an int
                    "','" +
                    "','" +
                    AllStaticData.Classification + "','" +
                    AllStaticData.ThreatName + "','" + //This is the ScenarioName in the database; named ThreatName here because TMSS
                    AllStaticData.Date + "','" +
                    AllStaticData.TmssIRSVersionNumber + "','" +
                    "'," +
                    AllStaticData.EarthGravityModel + ",'" + //EarthGravityModel is an int
                    "','" +
                    "'," +
                    AllStaticData.AtmosphericModel + "," + //Atmospheric model is an int
                    AllStaticData.Rotation + "," + //Rotation is an int
                    AllStaticData.RotationCoordinateFrame + ",'" + //RotationCoordinateFrame is an int
                    AllStaticData.EpochTime + "','" +
                    AllStaticData.SystemName + "','" +
                    "','" +
                    adp_name + "','" +
                    "'," +
                    AllStaticData.NumberPBVsOnMissile + "," + //NumberPBVsOnMissile is an int
                    AllStaticData.NumberRVsOnMissile + "," + //NumberRVsOnMissile is an int
                    AllStaticData.NumberObjectsOnMissile + "," + //NumberObjectsOnMissile is an int
                    AllStaticData.BboGroundRange + "," + //BboGroundRange is a float
                    "NULL," + //Ground Range is a float
                    AllStaticData.BboAltitude + "," + //BboAltitude is a float
                    AllStaticData.BboTime + ",'" + //BboTime is a float
                    "','" +
                    "','" +
                    "','" +
                    "'," +
                    AllStaticData.LaunchGeodeticLatitude + "," + //LaunchGeodeticLatitude is a float
                    AllStaticData.LaunchGeodeticLongitude + "," + //LaunchGeodeticLongitude is a float
                    AllStaticData.LaunchTime + "," + //LaunchTime is a float
                    AllStaticData.LaunchAzimuth + "," + //LaunchAzimuth is a float
                    "NULL," + //nulling the impact time; is a float.
                    "NULL,'" + //nulling the FlightTime; is a float
                    "','" +
                    "')";

                MySqlCommand cmdMain = new MySqlCommand(mainCommand, conn);
                MySqlDataReader rdrMain = cmdMain.ExecuteReader();

                //while (rdrMain.Read())
                //{
                //    Console.WriteLine(rdrMain[0]);
                //    //nextIndex = Int32.Parse(rdr[0].ToString());
                //    Console.WriteLine(rdrMain[0] + " -- " + rdrMain[1]);
                //}

                rdrMain.Close();

                string uid = "SELECT MAX(ID) FROM FLIGHTSTAGES";
                //FlightStages
                MySqlCommand uidCmd = new MySqlCommand(uid, conn);
                MySqlDataReader result = uidCmd.ExecuteReader();
                //6/8/11/15/20/26/33/34/35/37/40
                int fsuid = -1;
                while (result.Read())
                {
                    try
                    {
                        //Console.WriteLine(result[0]);
                        fsuid = Int32.Parse(result[0].ToString()) + 1;
                        //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        fsuid = 1;
                    }
                    catch (FormatException)
                    {
                        fsuid = 1;
                    }
                }

                result.Close();

                for (int i = 0; i < AllDynamicData.Count; i++)
                {
                    if (i != 0)
                    {
                        fsuid += 1;
                    }                    

                    string flightStagesStr = "INSERT INTO FLIGHTSTAGES (ID, " +
                        "MissileID, " + 
                        "ObjectsIncluded, " +
                        "ApogeeAltitude, " +
                        "ApogeeTime, " +
                        "ApogeeRange, " +
                        "AimpointName, " +
                        "AimpointLatitude, " +
                        "AimpointLongitude) VALUES (" +
                        fsuid + "," + 
                        nextIndex + ",'" + 
                        "'," +
                        AllDynamicData[i].ApogeeAltitude + "," +
                        AllDynamicData[i].ApogeeTime + "," +
                        "NULL,'" + //ApogeeRange is a float
                        "'," +
                        AllDynamicData[i].AimpointGeodeticLatitude + "," +
                        AllDynamicData[i].AimpointGeodeticLongitude + ")";

                    MySqlCommand cmdFlightStages = new MySqlCommand(flightStagesStr, conn);
                    MySqlDataReader rdrFlightStages = cmdFlightStages.ExecuteReader();

                    //while (rdrFlightStages.Read())
                    //{
                    //    Console.WriteLine(rdrFlightStages[0]);
                    //    //nextIndex = Int32.Parse(rdr[0].ToString());
                    //    Console.WriteLine(rdrFlightStages[0] + " -- " + rdrFlightStages[1]);
                    //}

                    rdrFlightStages.Close();
                }
                */
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
            try
            {
                string adp_name = GetADPNameFromSystemName(AllStaticData.SystemName);

                if (adp_name.Equals(""))
                {
                    throw new Exception("Unable to find adp_name for the missile type: " + AllStaticData.SystemName);
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.WriteLine("Filename," + AllStaticData.Filename);
                    file.WriteLine("Classification," + AllStaticData.Classification);
                    file.WriteLine("Scenario Name," + AllStaticData.ThreatName);
                    file.WriteLine("Earth Gravity Model," + AllStaticData.EarthGravityModel);
                    file.WriteLine("Atmospheric Model," + AllStaticData.AtmosphericModel);
                    file.WriteLine("Rotation," + AllStaticData.Rotation);
                    file.WriteLine("Rotation Coordinate Frame," + AllStaticData.RotationCoordinateFrame);
                    file.WriteLine("Epoch Time," + AllStaticData.EpochTime);
                    file.WriteLine("System Name," + AllStaticData.SystemName);
                    file.WriteLine("adp_name," + adp_name);
                    //file.WriteLine("Unique Allocation ID," + AllStaticData.UniqueAllocationId);

                    //file.WriteLine("\n\nHMI Detail: \n");
                    file.WriteLine("Number PBVs on Missile," + AllStaticData.NumberPBVsOnMissile);
                    file.WriteLine("Number RVs on Missile," + AllStaticData.NumberRVsOnMissile);
                    file.WriteLine("Number objects on Missile," + AllStaticData.NumberObjectsOnMissile);
                    file.WriteLine("BBO Ground Range," + AllStaticData.BboGroundRange);
                    file.WriteLine("BBO Altitude," + AllStaticData.BboAltitude);
                    file.WriteLine("BBO Time," + AllStaticData.BboTime);

                    file.WriteLine("Launch Geodetic Latitude," + AllStaticData.LaunchGeodeticLatitude);
                    file.WriteLine("Launch Geodetic Longitude," + AllStaticData.LaunchGeodeticLongitude);
                    //file.WriteLine("Launch Altitude," + AllStaticData.LaunchAltitude);
                    file.WriteLine("Launch Time," + AllStaticData.LaunchTime);
                    file.WriteLine("Launch Azimuth," + AllStaticData.LaunchAzimuth);
                    //file.WriteLine("Number of Object Sets to Follow," + AllStaticData.NumberOfObjectSetsToFollow);

                    //file.WriteLine("\n\nHMI Detail Dynamic Data: \n");
                    foreach (SmhrDynamicData dat in AllDynamicData)
                    {
                        file.WriteLine("Object name," + dat.ObjectName);
                        //file.WriteLine("Critical Path Object," + dat.CriticalPathObject);
                        file.WriteLine("Apogee Altitude," + dat.ApogeeAltitude);
                        file.WriteLine("Apogee Time," + dat.ApogeeTime);
                        //file.WriteLine("Aimpoint ID," + dat.AimpointId);
                        file.WriteLine("Aimpoint Latitude," + dat.AimpointGeodeticLatitude);
                        file.WriteLine("Aimpoint Longitude," + dat.AimpointGeodeticLongitude);
                    }
                }

                return true;
            } catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show("Error in writing to file:\n" + e.Message);
                return false;
            }
            */

            //The below code was used to write various data to a txt file. 
            /*
            if (!quitFlag)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.WriteLine("All Metadata: \n");

                    file.WriteLine("Filename," + AllStaticData.Filename);
                    file.WriteLine("Classification," + AllStaticData.Classification);
                    file.WriteLine("Threat Name," + AllStaticData.ThreatName);
                    file.WriteLine("System Name," + AllStaticData.SystemName);
                    file.WriteLine("Unique Allocation ID," + AllStaticData.UniqueAllocationId);

                    file.WriteLine("\n\nHMI Detail: \n");
                    file.WriteLine("Number PBVs on Missile," + AllStaticData.NumberPBVsOnMissile);
                    file.WriteLine("Number RVs on Missile," + AllStaticData.NumberRVsOnMissile);
                    file.WriteLine("Number objects on Missile," + AllStaticData.NumberObjectsOnMissile);
                    file.WriteLine("Launch Geodetic Latitude," + AllStaticData.LaunchGeodeticLatitude);
                    file.WriteLine("Launch Geodetic Longitude," + AllStaticData.LaunchGeodeticLongitude);
                    file.WriteLine("Launch Altitude," + AllStaticData.LaunchAltitude);
                    file.WriteLine("Launch Time," + AllStaticData.LaunchTime);
                    file.WriteLine("Launch Azimuth," + AllStaticData.LaunchAzimuth);
                    file.WriteLine("Number of Object Sets to Follow," + AllStaticData.NumberOfObjectSetsToFollow);

                    file.WriteLine("\n\nHMI Detail Dynamic Data: \n");
                    foreach (DynamicData dat in AllDynamicData)
                    {
                        file.WriteLine("\nObject name," + dat.ObjectName);
                        file.WriteLine("Critical Path Object," + dat.CriticalPathObject);
                        file.WriteLine("Apogee Altitude," + dat.ApogeeAltitude);
                        file.WriteLine("Apogee Time," + dat.ApogeeTime);
                        file.WriteLine("Aimpoint ID," + dat.AimpointId);
                        file.WriteLine("Aimpoint Latitude," + dat.AimpointGeodeticLatitude);
                        file.WriteLine("Aimpoint Longitude," + dat.AimpointGeodeticLongitude);
                    }
                }


            }
            */
        }

        private string GetADPNameFromSystemName(string missile_db_id)
        {
            string adp_name = "";

            string localPath = Environment.GetEnvironmentVariable("LocalAppData");

            XDocument aircElemxFile = XDocument.Load(localPath + "\\MAFTL\\ParserDatabaseFile\\ProcessedDatabaseFile.xml");

            string tempId;
            foreach (XElement entry in aircElemxFile.XPathSelectElement("//Entries").Descendants("Entry"))
            {
                tempId = entry.XPathSelectElement("missile_db_id").Value;
                
                if (tempId.Equals(missile_db_id))
                {
                    adp_name = entry.XPathSelectElement("adp_name").Value;
                    break;
                }
            }

            return adp_name;
        }

        private bool InputAllSmhrDataToDatabase(MySqlConnection conn, string adp_name)
        {
            //Tables with concerns:
            /*
                Classification@tbltrajectory
                Type@tblplatform
                Type@tblobject
                Classification@tblweapon
             */

            try
            {
                //First handle all tbl's that we care about, starting with ones that only contain static data
                
                int tblweaponID = HandleSingleRow(conn, "tblweapon", 0, 0, adp_name);
                int tbltrajectoryID = HandleSingleRow(conn, "tbltrajectory");

                tblplatformIDs = new List<int>();
                tblobjectIDs = new List<int>();
                //List<int> tblweaponIDs = new List<int>();

                for (int i = 0; i < AllDynamicData.Count; i++)
                {
                    tblobjectIDs.Add(HandleSingleRow(conn, "tblobject", i));
                    tblplatformIDs.Add(HandleSingleRow(conn, "tblplatform", i));
                }

                //int tblanalysisID = HandleSingleRow(conn, adp_name, "tblanalysis");
                //int tblscenarioID = HandleSingleRow(conn, adp_name, "tblscenario");
                //int tbltradestudyID = HandleSingleRow(conn, adp_name, "tbltradestudy", tblscenarioID);
                //HandleSingleRow(conn, adp_name, "tblobject");

                //Then do the tbm's that have only static data:
                int tbmtrajectory_weaponID = HandleSingleRow(conn, "tbmtrajectory_weapon", tblweaponID, tbltrajectoryID);
                _ = HandleSingleRow(conn, "tbmtrajectory_weapon_ballistic", tbmtrajectory_weaponID);
                for (int i = 0; i < AllDynamicData.Count; i++)
                {
                    _ = HandleSingleRow(conn, "tbmtrajectory_platform", i, tbltrajectoryID);
                    _ = HandleSingleRow(conn, "tbmtrajectory_object", tbltrajectoryID, i);
                    _ = HandleSingleRow(conn, "tbmweapon_object", i, tblweaponID);
                    _ = HandleSingleRow(conn, "tbmplatform_weapon", tblweaponID, i);
                }

                //Now do the tbs tables: 
                _ = HandleSingleRow(conn, "tbstrajectory_weapon_ballistic", tbmtrajectory_weaponID);
                _ = HandleSingleRow(conn, "tbssignatureofweapon", tblweaponID);
                //_ = HandleSingleRow(conn, "tbstrajectory_weapon_ballistic", tblweaponID);
                for (int i = 0; i < AllDynamicData.Count; i++)
                {
                    _ = HandleSingleRow(conn, "tbssignatureofobject", i);
                    _ = HandleSingleRow(conn, "tbssignatureofplatform", i);
                }
                
                return true; //Success!
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false; //Error!
            }
        }

        private int HandleSingleRow(MySqlConnection conn, string tableName, int otherVal = 0, int index = 0, string strVal = "")
        {
            int id = GetLowestUnusedID(conn, tableName);
            string command = GetCommandFromTable(tableName, id, otherVal, index, strVal);
            MySqlCommand cmd = new MySqlCommand(command, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();
            return id;
        }

        private string GetCommandFromTable(string tableName, int id, int val1 = 0, int val2 = 0, string strVal = "")
        {
            string command;

            switch (tableName)
            {
                case "tbltrajectory":
                    command = "INSERT INTO tbltrajectory (ID, " +
                        "Classification, " +
                        "Path, " +
                        "Filename, " +
                        "Filetype, " +
                        "Filesize, " +
                        "Spec, " +
                        "Version_Spec, " +
                        "Version_File, " +
                        "SourceApp, " +
                        "Scenario_smhr, " +
                        "TrajType, " +
                        "GravityModel, " +
                        "FlightMode, " +
                        "EarthShapeModel, " +
                        "AtmosphericModel, " +
                        "Rotation, " +
                        "RotationFrame, " +
                        "EpochTime, " +
                        "Range_Ground, " +
                        "LaunchRegion, " +
                        "LaunchCountry, " +
                        "LaunchName, " +
                        "AimRegion, " +
                        "AimCountry, " +
                        "AimName) VALUES (" +
                        id + ",'" + 
                        AllStaticData.Classification + "','" +
                        AllStaticData.FileLocation + "','" +
                        AllStaticData.Filename + "','" +
                        AllStaticData.FileType + "','" +
                        AllStaticData.FileSize + "','" + 
                        AllStaticData.SMHRVersionNumber + "','" + //!!!Not sure about Version_spec vs. Version_File vs. Spec
                        "','" + //!!Version_Spec is Varchar(22), ^
                        AllStaticData.TmssIRSVersionNumber + "','" + //!!!!!^
                        "','" +
                        AllStaticData.ThreatName + "','" +
                        "','" +
                        AllStaticData.EarthGravityModel + "','" +
                        "','" + 
                        "','" + 
                        AllStaticData.AtmosphericModel + "','" +
                        AllStaticData.Rotation + "','" +
                        AllStaticData.RotationCoordinateFrame + "','" +
                        AllStaticData.EpochTime + "'," +
                        "NULL,'" + //Range_Ground is a float
                        "','" + //LaunchRegion
                        "','" + //LaunchCountry
                        "','" + //LaunchName
                        "','" + //AimRegion
                        "','" + //AimCountry
                        "')";//AimName
                    break;
                case "tblobject":
                    command = "INSERT INTO tblobject (ID, " + 
                        "Type) VALUES (" +
                        id + ",'" + 
                        AllDynamicData[val1].ObjectName + "')"; 
                    break;
                case "tblplatform":
                    command = "INSERT INTO tblplatform (ID, Type) VALUES (" + id + ",'tempVal')";
                    //!!!This will need to change!!! Currently null because TMSS doesn't contain this value.
                    break;
                case "tbmtrajectory_platform":
                    command = "INSERT INTO tbmtrajectory_platform (ID," +
                        "ID_Platform," +
                        "ID_Trajectory) VALUES (" +
                        id + "," +
                        tblplatformIDs[val1] + "," +
                        val2 + ")";
                    break;
                case "tblanalysis":
                    command = "INSERT INTO tblanalysis (ID) VALUES (" + id + ")";
                    break;
                case "tblscenario":
                    command = "INSERT INTO tblscenario (ID) VALUES (" + id + ")";
                    break;
                case "tbltradestudy":
                    command = "INSERT INTO tbltradestudy (ID, ID_Scenario) VALUES (" + id + "," + val1 + ")";
                    break;
                case "tblweapon":
                    command = "INSERT INTO tblweapon (ID, " +
                        "Classification, " +
                        "Name_Generic, " +
                        "Name_TMSS, " +
                        "Name_ADP, " +
                        "Name_Intel, " +
                        "NumPBVs, " +
                        "NumRVs, " +
                        "NumObjs) VALUES (" +
                        id + ",'" +
                        AllStaticData.Classification + "'," +
                        "NULL, '" +
                        AllStaticData.SystemName + "','" +
                        strVal + "'," +
                        "NULL, " +
                        AllStaticData.NumberPBVsOnMissile + "," +
                        AllStaticData.NumberRVsOnMissile + "," +
                        AllStaticData.NumberObjectsOnMissile + ")";
                    break;
                case "tbmtrajectory_object":
                    command = "INSERT INTO tbmtrajectory_object (ID, " +
                        "ID_Trajectory, " +
                        "ID_Object, " +
                        "ApogeeAltitude, " +
                        "ApogeeTime, " +
                        "AimpointLat, " +
                        "AimpointLong) VALUES (" +
                        id + "," +
                        val1 + "," +
                        tblobjectIDs[val2] + "," +
                        AllDynamicData[val2].ApogeeAltitude + "," +
                        AllDynamicData[val2].ApogeeTime + "," +
                        AllDynamicData[val2].AimpointGeodeticLatitude + "," +
                        AllDynamicData[val2].AimpointGeodeticLongitude + ")";
                    break;
                case "tbmtrajectory_weapon":
                    command = "INSERT INTO tbmtrajectory_weapon (ID, " +
                        "ID_Weapon, " +
                        "ID_Trajectory, " +
                        "LaunchLat, " +
                        "LaunchLong, " +
                        "LaunchTime, " +
                        "LaunchAzimuth) VALUES (" +
                        id + "," +
                        val1 + "," +
                        val2 + "," +
                        AllStaticData.LaunchGeodeticLatitude + "," +
                        AllStaticData.LaunchGeodeticLongitude + "," +
                        AllStaticData.LaunchTime + "," +
                        AllStaticData.LaunchAzimuth + ")";
                    break;
                case "tbmweapon_object":
                    command = "INSERT INTO tbmweapon_object (ID, " +
                        "ID_Object, " +
                        "ID_Weapon) VALUES (" +
                        id + "," +
                        tblobjectIDs[val1] + "," +
                        val2 + ")";
                    break;
                case "tbmplatform_weapon":
                    command = "INSERT INTO tbmplatform_weapon (ID, " +
                        "ID_Weapon, " +
                        "ID_Platform) VALUES (" +
                        id + "," +
                        val1 + "," +
                        tblplatformIDs[val2] + ")";
                    break;
                case "tbmtrajectory_weapon_ballistic":
                    command = "INSERT INTO tbmtrajectory_weapon_ballistic (ID_Trajectory_Weapon, " +
                        "BBOGroundRange, " +
                        "BBOAltitude, " +
                        "BBOTime) VALUES (" +
                        id + "," +
                        AllStaticData.BboGroundRange + "," +
                        AllStaticData.BboAltitude + "," +
                        AllStaticData.BboTime + ")";
                    break;
                case "tbssignatureofobject":
                    command = "INSERT INTO tbssignatureofobject (ID, " +
                        "ID_Object, " +
                        "Type, " +
                        "FileLocation, " +
                        "FileSize, " +
                        "Waveband) VALUES (" +
                        id + "," +
                        tblobjectIDs[val1] + ",'','','','')";
                    break;
                case "tbssignatureofplatform":
                    command = "INSERT INTO tbssignatureofplatform (ID, " +
                        "ID_Platform, " +
                        "Type, " +
                        "FileLocation) VALUES (" +
                        id + "," +
                        tblplatformIDs[val1] + ",'','')";
                    break;
                case "tbssignatureofweapon":
                    command = "INSERT INTO tbssignatureofweapon (ID, " +
                        "Type, " +
                        "FileLocation, " +
                        "ID_Weapon) VALUES (" +
                        id + "," +
                        "''," +
                        "''," +
                        val1 + ")";
                    break;
                case "tbstrajectory_weapon_ballistic":
                    command = "INSERT INTO tbstrajectory_weapon_ballistic (ID_Trajectory_Weapon, " +
                        "BBOGroundRange, " +
                        "BBOAltitude, " +
                        "BBOTime) VALUES (" +
                        val1 + "," +
                        AllStaticData.BboGroundRange + "," +
                        AllStaticData.BboAltitude + "," +
                        AllStaticData.BboTime + ")";
                    break;
                default:
                    command = "";
                    break;
            }
            return command;
        }


        
    }
}
