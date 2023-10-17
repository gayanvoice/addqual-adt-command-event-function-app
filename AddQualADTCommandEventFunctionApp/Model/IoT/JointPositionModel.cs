using AddQualADTCommandEventFunctionApp.Model.DigitalTwins;

namespace AddQualADTCommandEventFunctionApp.Model.IoT
{
    public class JointPositionModel
    {
        public double _base { get; set; }
        public double _shoulder { get; set; }
        public double _elbow { get; set; }
        public double _wrist1 { get; set; }
        public double _wrist2 { get; set; }
        public double _wrist3 { get; set; }
        public static JointPositionModel Get(MoveJControlModel moveJControlModel)
        {
            JointPositionModel iotJointPositionModel = new JointPositionModel();
            iotJointPositionModel._base = moveJControlModel.Base;
            iotJointPositionModel._shoulder = moveJControlModel.Shoulder;
            iotJointPositionModel._elbow = moveJControlModel.Elbow;
            iotJointPositionModel._wrist1 = moveJControlModel.Wrist1;
            iotJointPositionModel._wrist2 = moveJControlModel.Wrist2;
            iotJointPositionModel._wrist3 = moveJControlModel.Wrist3;
            return iotJointPositionModel;
        }
    }
}