namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record XunXuUsePlan(LegalActionKind Action, IReadOnlyList<int> Targets, int? Card = null, CardKind? Required = null);
}
