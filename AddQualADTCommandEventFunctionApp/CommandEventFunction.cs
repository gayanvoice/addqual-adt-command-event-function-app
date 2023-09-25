// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.DigitalTwins.Core;
using AddQualADTCommandEventFunctionApp.Model.EventGrid;
using Azure;
using System.Threading.Tasks;
using AddQualADTCommandEventFunctionApp.Model.DigitalTwins;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Amqp.Framing;

namespace AddQualADTCommandEventFunctionApp
{
    public static class CommandEventFunction
    {
        private static readonly string ADT_SERVICE_URL = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        private static readonly string IOT_HUB_SERVICE_URL = Environment.GetEnvironmentVariable("IOT_HUB_SERVICE_URL");
        [FunctionName("CommandEventFunction")]
        public static async Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(ADT_SERVICE_URL), credential: defaultAzureCredential);

            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootObjectModel rootObjectModel = JsonConvert.DeserializeObject<RootObjectModel>(eventGridEvent.Data.ToString());
                if (rootObjectModel.Data.ModelId.Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                {
                    BasicDigitalTwin urCobotBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URCobot", digitalTwinsClient: digitalTwinsClient);
                    URCobotTwinModel urCobotModel = URCobotTwinModel.Get(urCobotBasicDigitalTwin);
                    log.LogInformation("UR COBOT EXECUTED");
                }
                else if (rootObjectModel.Data.ModelId.Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    BasicDigitalTwin urGripperBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URGripper", digitalTwinsClient: digitalTwinsClient);
                    URGripperTwinModel urGripperModel = URGripperTwinModel.Get(urGripperBasicDigitalTwin);
                    ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(IOT_HUB_SERVICE_URL);
                    CloudToDeviceMethod cloudToDeviceMethod;
                    if (urGripperModel.IsOpen) cloudToDeviceMethod = new CloudToDeviceMethod("OpenGripperCommand");
                    else cloudToDeviceMethod = new CloudToDeviceMethod("CloseGripperCommand");
                    cloudToDeviceMethod.ResponseTimeout = TimeSpan.FromSeconds(10);
                    CloudToDeviceMethodResult cloudToDeviceMethodResult = await serviceClient.InvokeDeviceMethodAsync("URGripper", cloudToDeviceMethod);
                    log.LogInformation("UR GRIPPER EXECUTED");
                }
                else
                {
                    log.LogInformation(eventGridEvent.Data.ToString());
                }
            }
        }
        private static async Task<BasicDigitalTwin> GetBasicDigitalTwinAsync(
           string twinId, DigitalTwinsClient digitalTwinsClient)
        {
            Response<BasicDigitalTwin> twinResponse = await digitalTwinsClient.GetDigitalTwinAsync<BasicDigitalTwin>(twinId);
            return twinResponse.Value;
        }
    }
}
