namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record ProgramOrdinaryTrickUseOption(
        ChoiceId Id,
        CardKind EffectiveCardKind,
        LegalActionKind ActionKind,
        IReadOnlyList<int> TargetSeats,
        int? TargetCardId,
        CardKind? RequiredCardKind,
        string Description)
    {
        public bool NextActualUseAdjusted { get; init; }
    }
}
