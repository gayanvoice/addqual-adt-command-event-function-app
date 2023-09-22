using Azure.DigitalTwins.Core;
using Newtonsoft.Json;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URCobotTwinModel
    {
        public bool IsInvoked { get; set; }
        public JointPositionModel? JointPosition { get; set; }
        public static URCobotTwinModel Get(BasicDigitalTwin basicDigitalTwin)
        {
            URCobotTwinModel urCobotTwinModel = new URCobotTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        if (property.Equals("JointPosition"))
                        {
                            JointPositionModel? jointPositionModel = JsonConvert.DeserializeObject<JointPositionModel>(value.ToString());
                            if (jointPositionModel != null) urCobotTwinModel.JointPosition = jointPositionModel;
                        }
                        if (property.Equals("IsInvoked"))
                        {
                            if (value is not null)
                            {
                                string? stringValue = value.ToString();
                                if (stringValue is not null)
                                {
                                    bool booleanValue = bool.Parse(stringValue);
                                    if (property.Equals("IsInvoked")) urCobotTwinModel.IsInvoked = booleanValue;
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
