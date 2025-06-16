/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public sealed class UserSettingsProfile : UserSettingsManager{
        readonly Resolution resolution = new("resolution");
        readonly Graphics graphics = new("graphics");
        readonly VSync vSync = new("vsync");
        readonly FPS fps = new("fps");
        readonly Sound sound = new("sound");
        protected override UserSetting[] CreateModules() => new UserSetting[] { resolution, graphics, vSync, fps, sound};
    }
}