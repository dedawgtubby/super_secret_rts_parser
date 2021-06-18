using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSParser
{
    class SmhrDynamicData
    {
        //Record 7
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string PbvAssociationNumber { get; set; }
        public string ObjectNumber { get; set; }
        public string ObjectCategory { get; set; }
        public string CriticalPathObject { get; set; }
        public string ParentObjectNumber { get; set; }
        public string ParentXVelocity { get; set; }
        public string ParentYVelocity { get; set; }
        public string ParentZVelocity { get; set; }
        public string BallisticCoefficientReferenceSource { get; set; }
        public string ObjectTypeDeploymentSequenceNumber { get; set; }

        //Record 8
        public string ObjectDestructionGeodeticLat { get; set; }
        public string ObjectDestructionGeodeticLon { get; set; }
        public string ObjectDestructionAlt { get; set; }
        public string ObjectDestructionTime { get; set; }
        public string ObjectReentryAngle { get; set; }
        public string ReentryTime { get; set; }
        public string ApogeeAltitude { get; set; }
        public string ApogeeTime { get; set; }

        //Record 9
        public string AimpointId { get; set; }
        public string AimpointGeodeticLatitude { get; set; }
        public string AimpointGeodeticLongitude { get; set; }
        public string AimpointAltitude { get; set; }
        public string NumberOfStatesToFollow { get; set; }

        //Record 10
        public List<String> Time { get; set; }
        public List<String> RVAssociationNumber { get; set; }
        public List<String> OnCriticalPathFlag { get; set; }
        public List<String> EventStatusFlag { get; set; }
        public List<String> EventFlag { get; set; }
        public List<String> EngineStatusFlag { get; set; }
        public List<String> GuidanceDirectionFlag { get; set; }
        public List<String> GuidanceManeuverMode { get; set; }
        public List<String> FuelFlowRate { get; set; }
        public List<String> BallisticCoefficient { get; set; }
        public List<String> AerodynamicMode { get; set; }
        public List<String> XPosition { get; set; }
        public List<String> YPosition { get; set; }
        public List<String> ZPosition { get; set; }

        //Record 11
        public List<String> XVelocity { get; set; }
        public List<String> YVelocity { get; set; }
        public List<String> ZVelocity { get; set; }
        public List<String> XAccelleration { get; set; }
        public List<String> YAccelleration { get; set; }
        public List<String> ZAccelleration { get; set; }
        public List<String> LateralAccelleration { get; set; }
        public List<String> AxialAccelleration { get; set; }
        public List<String> YBodyFrameAccelleration { get; set; }
        public List<String> ZBodyFrameAccelleration { get; set; }

        //Record 12
        public List<String> XBodyAxis { get; set; }
        public List<String> YBodyAxis { get; set; }
        public List<String> ZBodyAxis { get; set; }
        public List<String> OrientationMode { get; set; }

        //Record 13
        public List<String> XAxisEulerAnglePhi { get; set; }
        public List<String> YAxisEulerAngleTheta { get; set; }
        public List<String> ZAxisEulerAnglePsi { get; set; }
        public List<String> BodyXRollAxisAngularRate { get; set; }
        public List<String> BodyYPitchAxisAngularRate { get; set; }
        public List<String> BodyZYawAxisAngularRate { get; set; }
        public List<String> RollMomentOfInteriaIxx { get; set; }
        public List<String> PitchMomentOfInertiaIyy { get; set; }
        public List<String> YawMomentOfIntertiaIzz { get; set; }
        public List<String> RollPitchProductOfInertiaIxy { get; set; }
        public List<String> RollYawProductOfInertiaIxz { get; set; }
        public List<String> PitchYawProductOfInertiaIyz { get; set; }

        //Record 14
        public List<String> QuaternionVectorXElementQ1 { get; set; }
        public List<String> QuaternionVectorYElementQ2 { get; set; }
        public List<String> QuaternionVectorZElementQ3 { get; set; }
        public List<String> QuaternionScalarElementQ4 { get; set; }
        public List<String> QuaternionXElementQ1Rate { get; set; }
        public List<String> QuaternionYElementQ2Rate { get; set; }
        public List<String> QuaternionZElementQ3Rate { get; set; }
        public List<String> QuaternionElementQ4Rate { get; set; }

        //Record 15
        public List<String> InertialFlightPathAngle { get; set; }
        public List<String> PrecessionAngle { get; set; }
        public List<String> PrecessionRate { get; set; }
        public List<String> SpinAngle { get; set; }
        public List<String> SpinRate { get; set; }
        public List<String> CenterOfGravityLocationX { get; set; }
        public List<String> CenterOfGravityLocationY { get; set; }
        public List<String> CenterOfGravityLocationZ { get; set; }
        public List<String> TotalAngleOfAttack { get; set; }
        public List<String> PitchAngleOfAttack { get; set; }
        public List<String> YawAngleOfAttack { get; set; }

        //Record 16
        public List<String> Thrust { get; set; }
        public List<String> VacuumThrust { get; set; }
        public List<String> ThrustMode { get; set; }
        public List<String> ThrustDirectionX { get; set; }
        public List<String> ThrustDirectionY { get; set; }
        public List<String> ThrustDirectionZ { get; set; }

        //Record 17
        public List<String> Mass { get; set; }
        public List<String> LongitudinalCenterOfPressurePitch { get; set; }
        public List<String> StaticStabilityIndicator { get; set; }
        public List<String> LongitudinalCenterOfPressureYaw { get; set; }
        public List<String> DynamicPressure { get; set; }
        public List<String> MachNumber { get; set; }
        public List<String> DragCoefficient { get; set; }

        public SmhrDynamicData()
        {
            //No need to instantiate objects from records 7 - 9 because they're just strings. 

            //Record 10
            Time = new List<string>();
            RVAssociationNumber = new List<string>();
            OnCriticalPathFlag = new List<string>();
            EventStatusFlag = new List<string>();
            EventFlag = new List<string>();
            EngineStatusFlag = new List<string>();
            GuidanceDirectionFlag = new List<string>();
            GuidanceManeuverMode = new List<string>();
            FuelFlowRate = new List<string>();
            BallisticCoefficient = new List<string>();
            AerodynamicMode = new List<string>();
            XPosition = new List<string>();
            YPosition = new List<string>();
            ZPosition = new List<string>();

            //Record 11
            XVelocity = new List<string>();
            YVelocity = new List<string>();
            ZVelocity = new List<string>();
            XAccelleration = new List<string>();
            YAccelleration = new List<string>();
            ZAccelleration = new List<string>();
            LateralAccelleration = new List<string>();
            AxialAccelleration = new List<string>();
            YBodyFrameAccelleration = new List<string>();
            ZBodyFrameAccelleration = new List<string>();

            //Record 12
            XBodyAxis = new List<string>();
            YBodyAxis = new List<string>();
            ZBodyAxis = new List<string>();
            OrientationMode = new List<string>();

            //Record 13
            XAxisEulerAnglePhi = new List<string>();
            YAxisEulerAngleTheta = new List<string>();
            ZAxisEulerAnglePsi = new List<string>();
            BodyXRollAxisAngularRate = new List<string>();
            BodyYPitchAxisAngularRate = new List<string>();
            BodyZYawAxisAngularRate = new List<string>();
            RollMomentOfInteriaIxx = new List<string>();
            PitchMomentOfInertiaIyy = new List<string>();
            YawMomentOfIntertiaIzz = new List<string>();
            RollPitchProductOfInertiaIxy = new List<string>();
            RollYawProductOfInertiaIxz = new List<string>();
            PitchYawProductOfInertiaIyz = new List<string>();

            //Record 14
            QuaternionVectorXElementQ1 = new List<string>();
            QuaternionVectorYElementQ2 = new List<string>();
            QuaternionVectorZElementQ3 = new List<string>();
            QuaternionScalarElementQ4 = new List<string>();
            QuaternionXElementQ1Rate = new List<string>();
            QuaternionYElementQ2Rate = new List<string>();
            QuaternionZElementQ3Rate = new List<string>();
            QuaternionElementQ4Rate = new List<string>();

            //Record 15
            InertialFlightPathAngle = new List<string>();
            PrecessionAngle = new List<string>();
            PrecessionRate = new List<string>();
            SpinAngle = new List<string>();
            SpinRate = new List<string>();
            CenterOfGravityLocationX = new List<string>();
            CenterOfGravityLocationY = new List<string>();
            CenterOfGravityLocationZ = new List<string>();
            TotalAngleOfAttack = new List<string>();
            PitchAngleOfAttack = new List<string>();
            YawAngleOfAttack = new List<string>();

            //Record 16
            Thrust = new List<string>();
            VacuumThrust = new List<string>();
            ThrustMode = new List<string>();
            ThrustDirectionX = new List<string>();
            ThrustDirectionY = new List<string>();
            ThrustDirectionZ = new List<string>();

            //Record 17
            Mass = new List<string>();
            LongitudinalCenterOfPressurePitch = new List<string>();
            StaticStabilityIndicator = new List<string>();
            LongitudinalCenterOfPressureYaw = new List<string>();
            DynamicPressure = new List<string>();
            MachNumber = new List<string>();
            DragCoefficient = new List<string>();            
        }
    }
}
