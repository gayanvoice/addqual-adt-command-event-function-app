using Azure.DigitalTwins.Core;
using Newtonsoft.Json;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URCobotTwinModel
    {
        public bool IsMoving { get; set; }
        public JointPositionModel? ActualQJointPosition { get; set; }
        public static URCobotTwinModel Get(BasicDigitalTwin basicDigitalTwin)
        {
            URCobotTwinModel urCobotTwinModel = new URCobotTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        if (property.Equals("ActualQJointPosition"))
                        {
                            JointPositionModel actualQJointPositionModel = JsonConvert.DeserializeObject<JointPositionModel>(value.ToString());
                            if (actualQJointPositionModel != null) urCobotTwinModel.ActualQJointPosition = actualQJointPositionModel;
                        }
                        if (property.Equals("IsMoving"))
                        {
                            if (value is not null)
                            {
                                string? stringValue = value.ToString();
                                if (stringValue is not null)
                                {
                                    bool booleanValue = bool.Parse(stringValue);
                                    if (property.Equals("IsMoving")) urCobotTwinModel.IsMoving = booleanValue;
                                }
                            }
                        }
                    }
                }
            }
            return urCobotTwinModel;
        }
    }
}
