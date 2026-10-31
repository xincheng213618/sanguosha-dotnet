namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IHellEncounterHost CreateHellEncounterHost() => this.ComposeNativeHellEncounterHost();
}
