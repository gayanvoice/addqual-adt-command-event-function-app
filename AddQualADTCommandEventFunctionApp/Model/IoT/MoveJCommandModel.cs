using System.Collections.Generic;

namespace AddQualADTCommandEventFunctionApp.Model.IoT
{
    public class MoveJCommandModel
    {
        public double _acceleration { get; set; }
        public double _velocity { get; set; }
        public double _time_s { get; set; }
        public double _blend_radius { get; set; }
        public JointPositionModel[] _joint_position_model_array { get; set; }
        public static MoveJCommandModel Get(List<JointPositionModel> digitalTwinsJointPositionModelList)
        {
            MoveJCommandModel moveJCommandModel = new MoveJCommandModel();
            moveJCommandModel._acceleration = 0.5;
            moveJCommandModel._velocity = 0.5;
            moveJCommandModel._time_s = 0;
            moveJCommandModel._blend_radius = 0;
            moveJCommandModel._joint_position_model_array = digitalTwinsJointPositionModelList.ToArray();
            return moveJCommandModel;
        }
    }
}