namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record ShiScCompletedUse(long NativeId, CardActionContext Action, IReadOnlyList<int> Targets);
}
