using Azure.DigitalTwins.Core;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URGripperModel
    {
        public bool IsInvoked{ get; set; }
        public bool IsOpen{ get; set; }
        public static URGripperModel Get(BasicDigitalTwin basicDigitalTwin)
        {
            URGripperModel urGripperModel = new URGripperModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        string? stringValue = value.ToString();
                        if (stringValue is not null)
                        {
                            bool booleanValue = bool.Parse(stringValue);
                            if (property.Equals("IsInvoked")) urGripperModel.IsInvoked = booleanValue;
                            if (property.Equals("IsOpen")) urGripperModel.IsOpen = booleanValue;

                        }
                    }
                }
            }
            return urGripperModel;
        }
    }
}
