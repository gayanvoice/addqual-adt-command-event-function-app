using Azure.DigitalTwins.Core;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URGripperModel
    {
        public bool IsActive{ get; set; }
        public bool IsInvoked{ get; set; }
        public bool IsOpen{ get; set; }
        public static URGripperModel Get(BasicDigitalTwin basicDigitalTwin)
        {
            URGripperModel urGripperModel = new URGripperModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object? value))
                    if (value is not null)
                    {
                        if(property.Equals("IsActive")) urGripperModel.IsActive = true; 
                        else urGripperModel.IsActive = false;
                        if(property.Equals("IsInvoked")) urGripperModel.IsInvoked = true; 
                        else urGripperModel.IsInvoked = false;
                        if (property.Equals("IsOpen")) urGripperModel.IsOpen = true;
                        else urGripperModel.IsOpen = false;
                    }
            }
            return urGripperModel;
        }
    }
}
