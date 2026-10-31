namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeEncounterHost CreateModeEncounterHost() => this.ComposeNativeModeEncounterHost();
}
