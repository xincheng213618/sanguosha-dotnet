#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJiqiaoObserversRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 90)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 90)]
    internal bool CanContinueIssuedJiqiao(ProgramSkillFrame f)
    {
        if (f.JiqiaoPaid is not { } r)
            return false;
        var i = r.Invoice;
        if (i.Source.OwnerSeat != f.OwnerSeat || i.Source.SkillId != f.SkillId || i.Source.SkillInstanceId != f.SkillInstanceId || i.Source.BindingId != f.TriggerId || i.GameplayHash != f.GameplayHash || i.InstructionIndex != f.InstructionIndex || i.WindowId != f.WindowContext?.ParentFrameId || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor } || actor != f.OwnerSeat || i.ActualTurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { Op: SkillProgramEffectOp.DiscardEquipmentAndResolveJiqiao } e || e.SourceBind != i.CostBind || e.ResultBind != i.RevealBind || i.PaidCount <= 0 || i.CostBefore < 0 || i.CostAfter < i.CostBefore || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualCount < 0 || !Enum.IsDefined(r.Stage))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame w || w.Id != i.WindowId || w.OwnerSeat != f.OwnerSeat || w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.Items[w.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.BindingId != f.TriggerId || candidate.GameplayHash != f.GameplayHash)
            return false;
        var cost = f.CardSetBindings.SingleOrDefault(b => b.Name == i.CostBind);
        if (cost is null || cost.CardIds.Count != i.PaidCount || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JiqiaoCostPaidEvent>().Count(x => x.ProgramFrameId == f.Id && x.Invoice == i) != 1 || cost.CardIds.Any(cid => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > i.CostBefore && m.Sequence <= i.CostAfter && m.CardId == cid && m.From.OwnerSeat == f.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && IsJiqiaoPaidCostDestination(f, m) && m.Reason.Value == "program.jiqiao.cost") != 1))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JiqiaoStageIssuedEvent>().Count(x => x.ProgramFrameId == f.Id && x.Stage == r.Stage && x.Before == r.Before && x.After == r.After && x.ActualCount == r.ActualCount) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && IsExactJiqiaoStageMovement(f, r, m)) == r.ActualCount;
    }

    internal bool IsExactJiqiaoStageMovement(ProgramSkillFrame f, JiqiaoPaidPending r, CardMovementRecord m) => r.Stage switch
    {
        JiqiaoPaidStage.CostChildren => m.Reason.Value == "program.jiqiao.cost" && m.From.OwnerSeat == f.OwnerSeat && IsJiqiaoPaidCostDestination(f, m) && _coreProgramLifecycleOperations.GetProgramCardSet(f, r.Invoice.CostBind).CardIds.Contains(m.CardId),
        JiqiaoPaidStage.RevealChildren => m.Reason.Value == $"skill-program.{(f.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f))}.reveal" && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && _coreProgramLifecycleOperations.GetProgramCardSet(f, r.Invoice.RevealBind).CardIds.Contains(m.CardId),
        JiqiaoPaidStage.ClaimChildren => m.Reason.Value == "program.jiqiao.obtain" && m.From == CardLocation.Processing && m.To == CardLocation.Hand(f.OwnerSeat) && _coreProgramLifecycleOperations.GetProgramCardSet(f, r.Invoice.RevealBind).CardIds.Contains(m.CardId),
        JiqiaoPaidStage.CleanupChildren => m.Reason.Value == "program.jiqiao.remainder" && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && _coreProgramLifecycleOperations.GetProgramCardSet(f, r.Invoice.RevealBind).CardIds.Contains(m.CardId),
        _ => false
    };
    internal bool IsJiqiaoPaidCostDestination(ProgramSkillFrame frame, CardMovementRecord movement) => movement.To == CardLocation.DiscardPile || frame.JiqiaoPaid is { } pending && movement.From == CardLocation.Equipment(frame.OwnerSeat) && movement.Reason.Value == "program.jiqiao.cost" && movement.Sequence > pending.Invoice.CostBefore && movement.Sequence <= pending.Invoice.CostAfter && _coreProgramLifecycleOperations.GetProgramCardSet(frame, pending.Invoice.CostBind).CardIds.Contains(movement.CardId) && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Count(fact => fact.CardId == movement.CardId && fact.MovementSequence == movement.Sequence && fact.ParentFrameId == frame.Id) == 1;
    internal bool JiqiaoFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueIssuedJiqiao(root) || root.JiqiaoPaid is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && IsExactJiqiaoStageMovement(root, r, m));
        if (r.Stage != JiqiaoPaidStage.CostChildren)
            return false;
        // Removal can issue genuine Silver Lion recovery before its movement window.
        if (child is RecoveryReplacementFrame recovery)
            return recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion is { Producer: RecoveryAttemptProducer.SilverLion, MoveReason: { } moveReason } && moveReason.Value == "program.jiqiao.cost" && HasPaidJiqiaoSilverLion(root);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && HasPaidJiqiaoSilverLion(root);
        return false;
    }

    internal bool HasPaidJiqiaoSilverLion(ProgramSkillFrame f) => f.JiqiaoPaid is { } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Invoice.CostBefore && m.Sequence <= r.Invoice.CostAfter && m.From == CardLocation.Equipment(f.OwnerSeat) && IsJiqiaoPaidCostDestination(f, m) && m.Reason.Value == "program.jiqiao.cost" && _coreJiqiaoProgramsOperations.JiqiaoCard(m.CardId).Kind == CardKind.SilverLion);
    internal ProgramSkillFrame? JiqiaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.JiqiaoFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 150)]
    internal bool TryAdvanceJiqiaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && JiqiaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    SkillProgramStepOutcome global::CardGame.Core.IJiqiaoProgramHost.ExecutePaidJiqiao(ProgramSkillFrame f, SkillProgramEffect e) => _coreJiqiaoProgramsOperations.ExecutePaidJiqiao(f, e);
}
