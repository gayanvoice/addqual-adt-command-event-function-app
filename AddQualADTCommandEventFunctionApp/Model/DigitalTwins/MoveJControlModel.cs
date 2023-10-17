namespace AddQualADTCommandEventFunctionApp.Model
{
    public class MoveJControlModel
    {
        public double Base { get; set; }
        public double Shoulder { get; set; }
        public double Elbow { get; set; }
        public double Wrist1 { get; set; }
        public double Wrist2 { get; set; }
        public double Wrist3 { get; set; }
        public bool IsMoveJInvoked { get; set; }
    }
}