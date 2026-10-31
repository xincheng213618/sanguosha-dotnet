#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePairedHandRecastObserversRuntime
{
    internal bool ValidPairedHandRecastReceipt(ProgramSkillFrame f)
    {
        if (f.PairedHandRecast is not { } r || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.Owner.Seat != f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Partner.Seat) || r.Partner.Seat == f.OwnerSeat || f.SelectedCardIds is not [var original] || r.Owner.Material is not { } ownerMaterial || original != ownerMaterial.CardId || f.SelectedTargetSeats is not [var partner] || partner != r.Partner.Seat || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || r.PhaseInstanceId <= 0 || r.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || r.Cursor is < 0 or > 2 || !Enum.IsDefined(r.Stage) || _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id) != 0)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Activation is not { MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1, UsesPerPhase: 1, UsesPerTurn: null, UsesPerGame: null, TargetKind: SkillProgramTargetKind.OtherLivingWithHand } activation || activation.UsageGroup != r.UsageGroup || !activation.SourceZones.SequenceEqual([CardZoneKind.Hand]) || activation.Effects is not [{ Op: SkillProgramEffectOp.PairedHandRecast, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] || plan.Instructions is not [{ Op: SkillProgramEffectOp.PairedHandRecast }] || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, r.UsageGroup)) != 1 || r.EligiblePartnerMaterials.Count == 0 || r.EligiblePartnerMaterials.Select(m => m.CardId).Distinct().Count() != r.EligiblePartnerMaterials.Count || r.EligiblePartnerMaterials.Any(m => m.CardId <= 0 || !Enum.IsDefined(m.PrintedKind) || m.From != CardLocation.Hand(partner)) || r.Partner.Material is { } selected && !r.EligiblePartnerMaterials.Contains(selected))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || history.OfType<PairedHandRecastStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.UsageGroup != r.UsageGroup || start.PartnerSeat != partner || start.ActualTurnNumber != r.ActualTurnNumber || start.ActualTurnOwnerSeat != r.ActualTurnOwnerSeat || start.PhaseInstanceId != r.PhaseInstanceId)
            return false;
        bool ParticipantValid(PairedHandRecastParticipantReceipt p, int cursor)
        {
            if (p.Material is { } material && (material.CardId <= 0 || !Enum.IsDefined(material.PrintedKind) || material.From != CardLocation.Hand(p.Seat)))
                return false;
            var payments = history.OfType<PairedHandRecastPaidEvent>().Where(e => e.FrameId == f.Id && e.Cursor == cursor).ToArray();
            var draws = history.OfType<PairedHandRecastDrawIssuedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == cursor).ToArray();
            var skips = history.OfType<PairedHandRecastSkippedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == cursor).ToArray();
            if (!p.CostIssued)
                return !p.DrawIssued && p.CostBefore == 0 && p.CostAfter == 0 && p.CostBatchId is null && p.ActualDrawCount == 0 && p.DrawBefore == 0 && p.DrawAfter == 0 && payments.Length == 0 && draws.Length == 0 && (p.Skipped ? p.Material is not null && p.SkipReason is { } reason && Enum.IsDefined(reason) && skips is [var skipped] && skipped.ParticipantSeat == p.Seat && skipped.Reason == reason : p.SkipReason is null && skips.Length == 0);
            if (p.Material is not { } paid || p.Skipped || p.SkipReason is not null || skips.Length != 0 || p.CostBefore < 0 || p.CostAfter <= p.CostBefore || p.CostAfter > _corePairedHandRecastOperations.PairedHandRecastSequence || p.CostBatchId is not { } batch || payments is not [var invoice] || invoice.ParticipantSeat != p.Seat || invoice.CardId != paid.CardId || invoice.PrintedKind != paid.PrintedKind || invoice.From != paid.From || invoice.BatchId != batch || invoice.SequenceBefore != p.CostBefore || invoice.SequenceAfter != p.CostAfter)
                return false;
            var costs = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.CostBefore && m.Sequence <= p.CostAfter).ToArray();
            if (costs is not [var cost] || cost.CardId != paid.CardId || cost.CardKind != paid.PrintedKind || cost.From != paid.From || cost.To != CardLocation.DiscardPile || cost.Reason != CardMoveReasons.RecastDiscard)
                return false;
            if (!p.DrawIssued)
                return p.ActualDrawCount == 0 && p.DrawBefore == 0 && p.DrawAfter == 0 && draws.Length == 0;
            if (p.ActualDrawCount is < 0 or > 1 || p.DrawBefore < p.CostAfter || p.DrawAfter < p.DrawBefore || p.DrawAfter > _corePairedHandRecastOperations.PairedHandRecastSequence || draws is not [var draw] || draw.ParticipantSeat != p.Seat || draw.ActualCount != p.ActualDrawCount || draw.SequenceBefore != p.DrawBefore || draw.SequenceAfter != p.DrawAfter)
                return false;
            var rewards = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.DrawBefore && m.Sequence <= p.DrawAfter).ToArray();
            return rewards.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.Seat) && m.Reason == CardMoveReasons.RecastDraw) == p.ActualDrawCount && rewards.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.Seat) && m.Reason == CardMoveReasons.RecastDraw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
        }

        if (!ParticipantValid(r.Owner, 0) || !ParticipantValid(r.Partner, 1) || history.OfType<PairedHandRecastPaidEvent>().Any(e => e.FrameId == f.Id && e.Cursor is not (0 or 1)) || history.OfType<PairedHandRecastDrawIssuedEvent>().Any(e => e.FrameId == f.Id && e.Cursor is not (0 or 1)) || history.OfType<PairedHandRecastSkippedEvent>().Any(e => e.FrameId == f.Id && e.Cursor is not (0 or 1)))
            return false;
        var completions = history.OfType<PairedHandRecastCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == PairedHandRecastStage.ChoosingPartner)
            return r.Cursor == 0 && r.Partner.Material is null && !r.Owner.CostIssued && !r.Partner.CostIssued && !r.Owner.Skipped && !r.Partner.Skipped && f.PendingMovementContinuation is null && completions.Length == 0 && _corePairedHandRecastOperations.PairedRecastMaterialStillOwned(r.Owner) && r.EligiblePartnerMaterials.SequenceEqual(_corePairedHandRecastOperations.PairedHandMaterials(partner));
        if (r.Partner.Material is null)
            return false;
        for (var cursor = 0; cursor < 2; cursor++)
        {
            var p = cursor == 0 ? r.Owner : r.Partner;
            if (cursor < r.Cursor && !p.Skipped && (!p.CostIssued || !p.DrawIssued))
                return false;
            if (cursor > r.Cursor && (p.CostIssued || p.Skipped))
                return false;
        }

        if (r.Owner.DrawIssued && r.Partner.CostIssued && r.Partner.CostBefore < r.Owner.DrawAfter)
            return false;
        if (r.Stage == PairedHandRecastStage.Complete)
            return r.Cursor == 2 && f.PendingMovementContinuation is null && completions is [var complete] && complete.OwnerPaid == r.Owner.CostIssued && complete.PartnerPaid == r.Partner.CostIssued && complete.OwnerDrawCount == r.Owner.ActualDrawCount && complete.PartnerDrawCount == r.Partner.ActualDrawCount;
        if (completions.Length != 0)
            return false;
        if (r.Stage == PairedHandRecastStage.Ready)
            return f.PendingMovementContinuation is null && (r.Cursor == 2 || global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime.CurrentPairedRecast(r)is { CostIssued: false, Skipped: false });
        if (r.Cursor >= 2 || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime.CurrentPairedRecast(r).Seat)
            return false;
        var current = global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime.CurrentPairedRecast(r);
        return r.Stage == PairedHandRecastStage.CostChildren ? current.CostIssued && !current.DrawIssued : r.Stage == PairedHandRecastStage.DrawChildren && current.CostIssued && current.DrawIssued;
    }

    internal bool PairedHandRecastFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.PairedHandRecast is not { Stage: PairedHandRecastStage.CostChildren or PairedHandRecastStage.DrawChildren } r || !ValidPairedHandRecastReceipt(f) || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        var p = global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime.CurrentPairedRecast(r);
        var drawing = r.Stage == PairedHandRecastStage.DrawChildren;
        var before = drawing ? p.DrawBefore : p.CostBefore;
        var after = drawing ? p.DrawAfter : p.CostAfter;
        return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && (drawing || moved.Batch.Id == p.CostBatchId) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (drawing ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.Seat) && m.Reason == CardMoveReasons.RecastDraw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle : p.Material is { } paid && m.CardId == paid.CardId && m.CardKind == paid.PrintedKind && m.From == paid.From && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.RecastDiscard));
    }

    internal ProgramSkillFrame? PairedHandRecastObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.PairedHandRecastFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 100)]
    internal bool IsPairedHandRecastDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && PairedHandRecastObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 60)]
    internal bool TryAdvancePairedHandRecastSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || PairedHandRecastObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
