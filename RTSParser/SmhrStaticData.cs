using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    class SmhrStaticData
    {
        public string FileLocation { get; set; }
        public string Filename { get; set; }
        public string FileType { get; set; } = "smhr";
        public double FileSize { get; set; }

        //Record 0
        public string SMHRVersionNumber { get; set; }
        public string ApplicationUsed { get; set; }

        //Record 1
        public string Classification { get; set; }

        //Record 2
        public string ThreatName { get; set; }
        public string Date { get; set; }

        //Record 3
        public string TmssIRSVersionNumber { get; set; }

        //Record 4
        public string EarthGravityModel { get; set; }
        public string EarthGravityModelComponentN { get; set; }
        public string EarthGravityModelComponentM { get; set; }
        public string AtmosphericModel { get; set; }
        public string Rotation { get; set; }
        public string RotationCoordinateFrame { get; set; }
        public string ReentryAltitude { get; set; }
        public string EpochTime { get; set; }

        //Record 5
        public string MissileID { get; set; }
        public string SystemName { get; set; }
        public string SystemType { get; set; }
        public string SystemOperation { get; set; }
        public string NumberPBVsOnMissile { get; set; }
        public string NumberRVsOnMissile { get; set; }
        public string NumberObjectsOnMissile { get; set; }
        public string BboInertialRange { get; set; }
        public string BboGroundRange { get; set; }
        public string BboAltitude { get; set; }
        public string BboFlightPathAngle { get; set; }
        public string BboAzimuth { get; set; }

        //Record 6
        public string BboTime { get; set; }
        public string LiftOffTime { get; set; }
        public string LaunchComplex { get; set; }
        public string LaunchGeodeticLatitude { get; set; }
        public string LaunchGeodeticLongitude { get; set; }
        public string LaunchAltitude { get; set; }
        public string LaunchTime { get; set; }
        public string LaunchAzimuth { get; set; }
        public string NumberOfObjectSetsToFollow { get; set; }
        public string UniqueAllocationId { get; set; }
        public string RandomSeed { get; set; }

    }
}
