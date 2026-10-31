namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record MouJwSlashPlan(LegalAction Action, IReadOnlyList<int> Materials, PromptChoice? Multi = null);
}
