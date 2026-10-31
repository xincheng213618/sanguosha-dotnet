#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongOlRuntime
{
    internal const string OrdinaryBingyiDrawReason = "skill-program.gu-yong.bingyi.draw";
    internal const string OrdinaryBingyiShownBind = "ordinary-bingyi-shown-hand";
    internal bool HasOrdinaryGuYongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi);

    internal bool IsOrdinaryGuYongBingyiTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi }];
    internal static CardConversionSource OrdinaryGuYongSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_gu_yong_ol_OrdinaryGuYong.OrdinaryGuYongSource(f);
    internal bool OrdinaryBingyiPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiShownEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "ol:bingyi" && e.Phase == phase);
    internal bool IsOrdinaryBingyiDiscard(CardMovementRecord m, int owner) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)is { OwnerSeat: { } seat, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } && seat == owner;
    internal bool OrdinaryBingyiMovementRoot(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:bingyi" || c.BindingId != "ordinary-discard-reveal-share" || !IsOrdinaryGuYongBingyiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) || context.Window != SkillProgramTriggerWindow.DiscardPileReceived || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.MovementIndex != c.OccurrenceIndex || context.MovementBatch is not { DiscardRecoveryPhase: { } phase } batch || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || phase.TurnNumber != batch.TurnNumber || batch.Movements.Count == 0 || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || !IsOrdinaryBingyiDiscard(batch.Movements[c.OccurrenceIndex], c.OwnerSeat))
            return false;
        var first = batch.Movements.Select((m, i) => (m, i)).FirstOrDefault(x => IsOrdinaryBingyiDiscard(x.m, c.OwnerSeat));
        if (first.m is null || first.i != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.Batch.Id != batch.Id || parent.Batch.DiscardRecoveryPhase != phase || !parent.Batch.Movements.SequenceEqual(batch.Movements) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
            return false;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27598)]
    internal bool CanOfferOrdinaryBingyi(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!IsOrdinaryGuYongBingyiTrigger(trigger))
            return true;
        return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count > 0 && OrdinaryBingyiMovementRoot(c, context) && !OrdinaryBingyiPhaseSpent(c.OwnerSeat, context.MovementBatch!.DiscardRecoveryPhase!);
    }

    SkillProgramStepOutcome global::CardGame.Core.IOrdinaryGuYongHost.Bingyi(ProgramSkillFrame f) => BeginOrdinaryBingyi(f);
}
