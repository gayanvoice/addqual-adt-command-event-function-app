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
        //public static JointPositionModel Get(DigitalTwins.JointPositionModel digitalTwinsJointPositionModel)
        //{
        //    JointPositionModel iotJointPositionModel = new JointPositionModel();
        //    iotJointPositionModel._base = digitalTwinsJointPositionModel.Base;
        //    iotJointPositionModel._shoulder = digitalTwinsJointPositionModel.Shoulder;
        //    iotJointPositionModel._elbow = digitalTwinsJointPositionModel.Elbow;
        //    iotJointPositionModel._wrist1 = digitalTwinsJointPositionModel.Wrist1;
        //    iotJointPositionModel._wrist2 = digitalTwinsJointPositionModel.Wrist2;
        //    iotJointPositionModel._wrist3 = digitalTwinsJointPositionModel.Wrist3;
        //    return iotJointPositionModel;
        //}
    }
}