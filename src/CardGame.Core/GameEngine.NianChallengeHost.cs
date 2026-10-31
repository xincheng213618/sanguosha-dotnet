namespace CardGame.Core;

public sealed partial class GameEngine
{
    private INianChallengeHost CreateNianChallengeHost() => this.ComposeNativeNianChallengeHost();
}
