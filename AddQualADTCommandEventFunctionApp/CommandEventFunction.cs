// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.DigitalTwins.Core;
using AddQualADTCommandEventFunctionApp.Model;

namespace AddQualADTCommandEventFunctionApp
{
    public static class CommandEventFunction
    {
        private static readonly string ADT_SERVICE_URL = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        [FunctionName("CommandEventFunction")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(ADT_SERVICE_URL), credential: defaultAzureCredential);

            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootObjectModel rootObjectModel = JsonConvert.DeserializeObject<RootObjectModel>(eventGridEvent.Data.ToString());
                if (rootObjectModel.Data.ModelId.Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                {
                    log.LogInformation("Execute URCobot");
                }
                else if (rootObjectModel.Data.ModelId.Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    log.LogInformation("Execute URGripper");
                }
                else
                {
                    log.LogInformation(eventGridEvent.Data.ToString());
                }
                //JObject jObject = JsonConvert.DeserializeObject<JObject>(eventGridEvent.Data.ToString());
                //log.LogInformation(jObject["dataschema"].ToString());

                //if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                //{
                //    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                //    Azure.JsonPatchDocument azureJsonPatchDocument = new Azure.JsonPatchDocument();
                //    JointPositionModel jointPositionModel = JointPositionModel.GetDegrees(urCobotModel);
                //    azureJsonPatchDocument.AppendAdd("/IsPaused", true);
                //    azureJsonPatchDocument.AppendAdd("/IsSafetyPopupClosed", true);
                //    azureJsonPatchDocument.AppendAdd("/IsProtectiveStopUnlocked", true);
                //    azureJsonPatchDocument.AppendAdd("/IsPowerOn", true);
                //    azureJsonPatchDocument.AppendAdd("/IsFreeDriveModeEnabled", true);
                //    azureJsonPatchDocument.AppendAdd("/IsTeachModeEnabled", true);
                //    azureJsonPatchDocument.AppendAdd("/JointPosition", jointPositionModel);
                //    azureJsonPatchDocument.AppendAdd("/IsInvoked", false);
                //    await digitalTwinsClient.UpdateDigitalTwinAsync("URCobot", azureJsonPatchDocument);
                //}
                //else if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                //{
                //    URGripperModel urGripperModel = JsonConvert.DeserializeObject<URGripperModel>(eventGridEvent.Data.ToString());
                //    Azure.JsonPatchDocument azureJsonPatchDocument = new Azure.JsonPatchDocument();
                //    azureJsonPatchDocument.AppendAdd("/IsActive", urGripperModel.data.ACT);
                //    if (urGripperModel.data.ACT == 1) azureJsonPatchDocument.AppendAdd("/IsActive", true);
                //    else azureJsonPatchDocument.AppendAdd("/IsActive", false);
                //    if (urGripperModel.data.POS < 10) azureJsonPatchDocument.AppendAdd("/IsOpen", true);
                //    else azureJsonPatchDocument.AppendAdd("/IsOpen", false);
                //    azureJsonPatchDocument.AppendAdd("/IsInvoked", false);
                //    await digitalTwinsClient.UpdateDigitalTwinAsync("URGripper", azureJsonPatchDocument);
                //}
            }
        }
    }
}
