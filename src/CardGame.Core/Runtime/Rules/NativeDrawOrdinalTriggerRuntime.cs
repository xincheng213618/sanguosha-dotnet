using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_NativeDrawOrdinals;

namespace CardGame.Core;

// Trigger admission consumes the completed invoice without becoming part of
// ordinal issuance or owning a second draw/window state.
[ProgramRuntimeModule]
internal sealed class NativeDrawOrdinalTriggerRuntime(INativeDrawInvoiceQueries invoices,
    ICardMovementHistoryQueries movements, IProgramRuntimeFrames frames)
{
    internal int[] MatchingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.NativeDrawInvocation is not { ActualTurnDrawOrdinal: > 0, OrdinalEntitlements: { } sources } proof ||
            proof.OwnerSeat != candidate.OwnerSeat || proof.LastBatchId != batch.Id || proof.ParentFrameId != batch.ParentFrameId ||
            proof.ActualTurnNumber != batch.TurnNumber || !invoices.IsValid(proof) ||
            !sources.Contains(new(new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash)))
            return [];
        var last = proof.Materials[^1];
        return batch.Movements.Select((movement, index) => (movement, index)).Where(item =>
            item.movement.Sequence == last.MovementSequence && item.movement.CardId == last.CardId &&
            item.movement.CardKind == last.PrintedKind && item.movement.From == CardLocation.DrawPile &&
            item.movement.To == CardLocation.Hand(candidate.OwnerSeat) && item.movement.Reason == proof.Reason &&
            movements.Read().Contains(item.movement)).Select(item => item.index).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 27910)]
    private bool CanRunNativeDrawOrdinalTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context)
    {
        if (!UsesNativeDrawOrdinal(trigger)) return true;
        return context.Window == SkillProgramTriggerWindow.CardsGained && context.MovementBatch is { } batch &&
            frames.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is { } parent &&
            parent.Batch.Id == batch.Id && parent.Batch.Movements.SequenceEqual(batch.Movements) &&
            parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate &&
            MatchingIndexes(batch, candidate).Length == 1 &&
            context.Facts?.NativeDrawActualTurnOrdinal == batch.NativeDrawInvocation!.ActualTurnDrawOrdinal;
    }
}
