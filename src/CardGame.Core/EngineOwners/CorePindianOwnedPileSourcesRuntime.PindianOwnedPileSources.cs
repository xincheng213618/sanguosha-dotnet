#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePindianOwnedPileSourcesRuntime
{
    internal bool HasPaidOwnedPilePindianSource(PindianFrame frame) => frame.OwnedPileSource is { } source && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > source.SequenceBefore && m.CardId == source.CardId && m.From == source.Pile.Location && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.PindianReveal) == 1;
    internal void AssertOwnedPilePindianSource(PindianFrame frame)
    {
        if (frame.OwnedPileSource is not { } source)
            return;
        if (source.OriginProgramFrameId != frame.ParentFrameId || source.Origin.OwnerSeat != frame.SourceSeat || source.Pile.OwnerSeat != frame.SourceSeat || source.CardId != frame.SourceCardId || frame.SourceUsesDrawPileTop || source.SequenceBefore < 0 || source.SequenceBefore > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || frame.OpponentSeat is null || frame.PindianStep is PindianStep.ChooseParticipants or PindianStep.ChooseSourceCard || !_characterGuanHaiOlOperations.IsGuanHaiPindianSource(frame, HasPaidOwnedPilePindianSource(frame)))
            throw new InvalidOperationException("A pile Pindian lost its issued original source, entity or native payment.");
    }
}
