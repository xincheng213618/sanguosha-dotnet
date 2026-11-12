#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal bool ValidHappyWeiYanDraw(NativeDrawOutcome draw, int owner, long frameId)
    {
        if (draw.RecipientSeat != owner || draw.RequestedCount != 2 || draw.SequenceBefore < 0 || draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || draw.ActualDrawIds.Count > 2 || draw.ActualDrawIds.Distinct().Count() != draw.ActualDrawIds.Count || draw.ActualDrawIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.Reason.Value == HappyEzhiDraw) != 1))
            return false;
        if (draw.AttemptFrameId is { } attempt)
            return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptStartedEvent>().Count(e => e.Context.AttemptFrameId == attempt && e.Context.ParentFrameId == frameId && e.Context.RecipientSeat == owner && e.Context.RequestedCount == 2 && e.Context.Reason.Value == HappyEzhiDraw && e.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation && e.Return.InstructionIndex == 1) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frameId && e.RecipientSeat == owner && e.RequestedCount == 2 && e.Replaced == (draw.Replacement is not null)) == 1;
        return draw.Replacement is null;
    }
}
