namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record ShenDaRepeatPlan(string Id, CardKind Kind, LegalActionKind ActionKind,
        IReadOnlyList<int> TargetSeats, string Description, int? TargetCardId = null, CardKind? RequiredKind = null);
}
