namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeSetupEquipmentHost CreateModeSetupEquipmentHost() => this.ComposeNativeModeSetupEquipmentHost();
}
