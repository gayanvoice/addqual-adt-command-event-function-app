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
using AddQualADTCommandEventFunctionApp.Model.IoT;
using System.Collections.Generic;

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
                    URCobotTwinModel urCobotTwinModel = URCobotTwinModel.GetFromBasicDigitalTwin(urCobotBasicDigitalTwin);
                    if (urCobotTwinModel.StartFreeDriveControlModel.IsStartFreeDrive)
                    {
                        var payload = new
                        {
                            Value = DateTime.Now.ToString(),
                        };
                        ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(IOT_HUB_SERVICE_URL);
                        CloudToDeviceMethod cloudToDeviceMethod = new CloudToDeviceMethod("StartFreeDriveModeCommand");
                        cloudToDeviceMethod.SetPayloadJson(JsonConvert.SerializeObject(payload));
                        CloudToDeviceMethodResult cloudToDeviceMethodResult = await serviceClient.InvokeDeviceMethodAsync("URCobot", cloudToDeviceMethod);
                        log.LogInformation(JsonConvert.SerializeObject(cloudToDeviceMethodResult));
                        log.LogInformation("UR COBOT EXECUTED: START FREE DRIVE CONTROL");
                    }
                    else if (urCobotTwinModel.StopFreeDriveControlModel.IsStopFreeDrive)
                    {
                        var payload = new
                        {
                            Value = DateTime.Now.ToString(),
                        };
                        ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(IOT_HUB_SERVICE_URL);
                        CloudToDeviceMethod cloudToDeviceMethod = new CloudToDeviceMethod("StopFreeDriveModeCommand");
                        cloudToDeviceMethod.SetPayloadJson(JsonConvert.SerializeObject(payload));
                        CloudToDeviceMethodResult cloudToDeviceMethodResult = await serviceClient.InvokeDeviceMethodAsync("URCobot", cloudToDeviceMethod);
                        log.LogInformation(JsonConvert.SerializeObject(cloudToDeviceMethodResult));
                        log.LogInformation("UR COBOT EXECUTED: STOP FREE DRIVE CONTROL");
                    }
                    else if (urCobotTwinModel.MoveJControlModel.IsMoveJInvoked)
                    {
                        JointPositionModel jointPositionModel = JointPositionModel.Get(urCobotTwinModel.MoveJControlModel);
                        List<JointPositionModel> digitalTwinsJointPositionModelList = new List<JointPositionModel>
                        {
                            jointPositionModel
                        };
                        MoveJCommandModel moveJCommandModel = MoveJCommandModel.Get(digitalTwinsJointPositionModelList: digitalTwinsJointPositionModelList);
                        string moveJCommandPayload = JsonConvert.SerializeObject(moveJCommandModel);
                        ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(IOT_HUB_SERVICE_URL);
                        CloudToDeviceMethod cloudToDeviceMethod = new CloudToDeviceMethod("MoveJCommand");
                        cloudToDeviceMethod.SetPayloadJson(moveJCommandPayload);
                        CloudToDeviceMethodResult cloudToDeviceMethodResult = await serviceClient.InvokeDeviceMethodAsync("URCobot", cloudToDeviceMethod);
                        log.LogInformation(JsonConvert.SerializeObject(cloudToDeviceMethodResult));
                        log.LogInformation("UR COBOT EXECUTED: MOVE J CONTROL" + JsonConvert.SerializeObject(moveJCommandModel));
                    }
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