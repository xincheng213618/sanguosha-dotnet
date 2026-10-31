#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuHappyGodRuntime
{
    internal bool HappyLuBuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ObtainHappyShenLuBuTurnBasic || f.HappyLuBu is not { Stage: HappyLuBuStage.BasicChildren, Paid.Count: 1 } r || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        return r.After > r.Before;
    }

    internal bool HappyLuBuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => f.HappyLuBu is { Stage: HappyLuBuStage.BasicChildren } r && f.PendingMovementContinuation is { } pending && HappyLuBuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(r.Paid.Contains);
    internal ProgramSkillFrame? HappyLuBuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HappyLuBuFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 41200)]
    internal bool HappyLuBuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HappyLuBuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 41200)]
    internal bool AdvanceHappyLuBuObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyLuBuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41200)]
    internal bool RetainHappyLuBuPaid(ProgramSkillFrame f)
    {
        if (f.HappyLuBu is { Paid.Count: > 0 })
        {
            return true;
        }

        return f.SkillId == HappyLuBuComposition.Shenfen && f.TriggerId is null && f.ActivationId == "limited-destruction" && f.InstructionIndex >= 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 3) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 41200)]
    internal void AssertHappyLuBuUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.HappyLuBuWuqian is not null))
        {
            var r = use.HappyLuBuWuqian!;
            if (r.UseId != use.Id || r.ActionId != use.Action?.ActionId || r.Source.OwnerSeat != use.SourceSeat || r.Source.SkillId != HappyLuBuComposition.Wuqian || r.Source.BindingId != "first-kind-use" || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.OriginalTargets.Count < 1 || r.OriginalTargets.Distinct().Count() != r.OriginalTargets.Count || r.AddedTarget is { } extra && (r.OriginalTargets.Contains(extra) || _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentCardEnhancedEvent>().Count(e => e.CardUseFrameId == use.Id && e.CardActionId == r.ActionId && e.OwnerSeat == r.Source.OwnerSeat && e.Enhancement == CurrentCardEnhancement.ExtraTarget && e.ExtraTargetSeat == extra) != 1) || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuStartedEvent>().Count(e => e == new HappyLuBuStartedEvent(r.ProgramId, r.Source, r.GameplayHash, SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse, r.WindowId, r.ActualTurn, use.Id, r.ActionId)) != 1 || r.Applied && (_coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuUseAppliedEvent>().Count(e => e == new HappyLuBuUseAppliedEvent(r.ProgramId, use.Id, r.ActionId, r.AddedTarget)) != 1 || !use.IgnoresArmor || (use.Enhancements & CurrentCardEnhancement.IgnoreArmor) == 0))
                throw new InvalidOperationException("Wuqian lost the frozen issued real-use policy and exact source proof.");
        }
    }

    internal bool IsHappyLuBuTrueVirtualProducer(CardUseFrame use, ProgramSkillFrame parent, SkillProgramEffect paused)
    {
        if (paused is not { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash, UseCardActionWindows: true } || use.HappyLuBuWuqian is not { Applied: true, Kind: CardKind.Slash, OriginalTargets: [var original], AddedTarget: { } extra } r || use.Action is not { } action || r.UseId != use.Id || r.ActionId != action.ActionId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.Source.OwnerSeat != use.SourceSeat || r.Source.SkillId != HappyLuBuComposition.Wuqian || r.Source.BindingId != "first-kind-use" || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.GameplayHash || r.ProgramId == parent.Id || parent.OwnerSeat != use.SourceSeat || parent.SelectedTargetSeats is not [var selected] || selected != original || original == extra || !action.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) || !HappyLuBuFirstUse(use))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuStartedEvent>().Count(e => e == new HappyLuBuStartedEvent(r.ProgramId, r.Source, r.GameplayHash, SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse, r.WindowId, r.ActualTurn, use.Id, r.ActionId)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == r.ProgramId && e.OwnerSeat == r.Source.OwnerSeat && e.SkillId == r.Source.SkillId && e.BindingId == r.Source.BindingId && e.SkillInstanceId == r.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuUseAppliedEvent>().Count(e => e == new HappyLuBuUseAppliedEvent(r.ProgramId, use.Id, r.ActionId, extra)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentCardEnhancedEvent>().Count(e => e.CardUseFrameId == use.Id && e.CardActionId == r.ActionId && e.OwnerSeat == r.Source.OwnerSeat && e.Enhancement == CurrentCardEnhancement.ExtraTarget && e.ExtraTargetSeat == extra) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == CardKind.Slash && e.SourceSeat == use.SourceSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<TargetsConfirmedEvent>().Count(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([original])) == 1;
    }
}
