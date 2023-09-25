using Azure.DigitalTwins.Core;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URGripperTwinModel
    {
        public bool IsOpen { get; set; }
        public bool IsInvoked { get; set; }
        public static URGripperTwinModel GetFromBasicDigitalTwin(BasicDigitalTwin basicDigitalTwin)
        {
            URGripperTwinModel urGripperTwinModel = new URGripperTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        string stringValue = value.ToString();
                        if (stringValue is not null)
                        {
                            bool boolValue = bool.Parse(stringValue);
                            if (property.Equals("IsInvoked")) urGripperTwinModel.IsInvoked = boolValue;
                            if (property.Equals("IsOpen")) urGripperTwinModel.IsOpen = boolValue;

                        }
                    }
                }
            }
            return urGripperTwinModel;
        }
}
