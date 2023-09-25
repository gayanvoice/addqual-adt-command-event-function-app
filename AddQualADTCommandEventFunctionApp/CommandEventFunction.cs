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
                    if (urCobotTwinModel.IsInvoked)
                    {
                        Model.IoT.JointPositionModel jointPositionModel = Model.IoT.JointPositionModel.Get(urCobotTwinModel.ActualQJointPosition);
                        List<Model.IoT.JointPositionModel> digitalTwinsJointPositionModelList = new List<Model.IoT.JointPositionModel>
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
                        log.LogInformation("UR COBOT EXECUTED" + JsonConvert.SerializeObject(moveJCommandModel));
                    }
                    else
                    {
                        log.LogInformation("UR COBOT NOT EXECUTED");
                    }
                }
                else if (rootObjectModel.Data.ModelId.Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    BasicDigitalTwin urGripperBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URGripper", digitalTwinsClient: digitalTwinsClient);
                    URGripperTwinModel urGripperTwinModel = URGripperTwinModel.GetFromBasicDigitalTwin(urGripperBasicDigitalTwin);
                    if (urGripperTwinModel.IsInvoked)
                    {
                        ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(IOT_HUB_SERVICE_URL);
                        CloudToDeviceMethod cloudToDeviceMethod;
                        if (urGripperTwinModel.IsOpen) cloudToDeviceMethod = new CloudToDeviceMethod("OpenGripperCommand");
                        else cloudToDeviceMethod = new CloudToDeviceMethod("CloseGripperCommand");
                        cloudToDeviceMethod.ResponseTimeout = TimeSpan.FromSeconds(10);
                        CloudToDeviceMethodResult cloudToDeviceMethodResult = await serviceClient.InvokeDeviceMethodAsync("URGripper", cloudToDeviceMethod);
                        log.LogInformation(JsonConvert.SerializeObject(cloudToDeviceMethodResult));
                        log.LogInformation("UR GRIPPER EXECUTED");
                    }
                    else
                    {
                        log.LogInformation("UR GRIPPER NOT EXECUTED");
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