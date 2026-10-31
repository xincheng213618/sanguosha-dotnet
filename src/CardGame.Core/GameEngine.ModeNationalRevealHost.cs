namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeNationalRevealHost CreateModeNationalRevealHost() => this.ComposeNativeModeNationalRevealHost();
}
