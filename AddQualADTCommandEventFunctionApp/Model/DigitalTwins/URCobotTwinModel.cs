using Azure.DigitalTwins.Core;
using Newtonsoft.Json;

namespace AddQualADTCommandEventFunctionApp.Model.DigitalTwins
{
    public class URCobotTwinModel
    {
        public MoveJControlModel MoveJControlModel { get; set; }
        public ClosePopupControlModel ClosePopupControlModel { get; set; }
        public CloseSafetyPopupControlModel CloseSafetyPopupControlModel { get; set; }
        public OpenPopupControlModel OpenPopupControlModel { get; set; }
        public PauseControlModel PauseControlModel { get; set; }
        public PlayControlModel PlayControlModel { get; set; }
        public PowerOffControlModel PowerOffControlModel { get; set; }
        public PowerOnControlModel PowerOnControlModel { get; set; }
        public StartFreeDriveControlModel StartFreeDriveControlModel { get; set; }
        public StopFreeDriveControlModel StopFreeDriveControlModel { get; set; }
        public UnlockProtectiveStopControlModel UnlockProtectiveStopControlModel { get; set; }
        public static URCobotTwinModel GetFromBasicDigitalTwin(BasicDigitalTwin basicDigitalTwin)
        {
            URCobotTwinModel urCobotTwinModel = new URCobotTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        switch (property)
                        {
                            case "MoveJControl":
                                MoveJControlModel moveJControlModel = JsonConvert.DeserializeObject<MoveJControlModel>(value.ToString());
                                if (moveJControlModel != null) urCobotTwinModel.MoveJControlModel = moveJControlModel;
                                break;
                            case "ClosePopupControl":
                                ClosePopupControlModel closePopupControlModel = JsonConvert.DeserializeObject<ClosePopupControlModel>(value.ToString());
                                if (closePopupControlModel != null) urCobotTwinModel.ClosePopupControlModel = closePopupControlModel;
                                break;
                            case "CloseSafetyPopupControl":
                                CloseSafetyPopupControlModel closeSafetyPopupControlModel = JsonConvert.DeserializeObject<CloseSafetyPopupControlModel>(value.ToString());
                                if (closeSafetyPopupControlModel != null) urCobotTwinModel.CloseSafetyPopupControlModel = closeSafetyPopupControlModel;
                                break;
                            case "OpenPopupControl":
                                OpenPopupControlModel openPopupControlModel = JsonConvert.DeserializeObject<OpenPopupControlModel>(value.ToString());
                                if (openPopupControlModel != null) urCobotTwinModel.OpenPopupControlModel = openPopupControlModel;
                                break;
                            case "PauseControl":
                                PauseControlModel pauseControlModel = JsonConvert.DeserializeObject<PauseControlModel>(value.ToString());
                                if (pauseControlModel != null) urCobotTwinModel.PauseControlModel = pauseControlModel;
                                break;
                            case "PlayControl":
                                PlayControlModel playControlModel = JsonConvert.DeserializeObject<PlayControlModel>(value.ToString());
                                if (playControlModel != null) urCobotTwinModel.PlayControlModel = playControlModel;
                                break;
                            case "PowerOffControl":
                                PowerOffControlModel powerOffControlModel = JsonConvert.DeserializeObject<PowerOffControlModel>(value.ToString());
                                if (powerOffControlModel != null) urCobotTwinModel.PowerOffControlModel = powerOffControlModel;
                                break;
                            case "PowerOnControl":
                                PowerOnControlModel powerOnControlModel = JsonConvert.DeserializeObject<PowerOnControlModel>(value.ToString());
                                if (powerOnControlModel != null) urCobotTwinModel.PowerOnControlModel = powerOnControlModel;
                                break;
                            case "StartFreeDriveControl":
                                StartFreeDriveControlModel startFreeDriveControlModel = JsonConvert.DeserializeObject<StartFreeDriveControlModel>(value.ToString());
                                if (startFreeDriveControlModel != null) urCobotTwinModel.StartFreeDriveControlModel = startFreeDriveControlModel;
                                break;
                            case "StopFreeDriveControl":
                                StopFreeDriveControlModel stopFreeDriveControlModel = JsonConvert.DeserializeObject<StopFreeDriveControlModel>(value.ToString());
                                if (stopFreeDriveControlModel != null) urCobotTwinModel.StopFreeDriveControlModel = stopFreeDriveControlModel;
                                break;
                            case "UnlockProtectiveStopControl":
                                UnlockProtectiveStopControlModel unlockProtectiveStopControlModel = JsonConvert.DeserializeObject<UnlockProtectiveStopControlModel>(value.ToString());
                                if (unlockProtectiveStopControlModel != null) urCobotTwinModel.UnlockProtectiveStopControlModel = unlockProtectiveStopControlModel;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return urCobotTwinModel;
        }
    }
}