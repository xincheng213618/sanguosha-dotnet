#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal bool HasMouHuangYueyingBingcaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangYueyingBingcai);

    internal bool IsMouBingcaiFirstBasicAction(CardUseFrame use, CardActionContext action)
    {
        if (!HasMouHuangYueyingBingcaiCapability || !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Basic)
            return false;
        var first = _coreWholeActualTurnUsesOperations.WholeActualTurnUseHistory().FirstOrDefault(e => e.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic);
        return first is { WholeActualTurnUseSequence: > 0, RoundActionType: CardActionType.Use } && first.OriginFrameId == use.Id && !first.IsNullificationUse && (first.CardActionId == action.ActionId || first.CardActionId is null && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use));
    }

    internal bool TryGetMouBingcaiFirstBasic(ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) != context.Window || w.ParentFrameId != identity.ParentCardUseFrameId || w.Action.ActionId != identity.CardActionId || w.Action.EffectiveKind != identity.EffectiveKind || w.Action.ActorSeat != identity.ActorSeat || w.Action.Type != CardActionType.Use || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { } action } actual || actual.Action.ActionId != w.Action.ActionId || actual.SourceSeat != action.ActorSeat || action.EffectiveKind != actual.CardKind || !IsMouBingcaiFirstBasicAction(actual, action))
            return false;
        use = actual;
        window = w;
        return true;
    }

    internal bool TryBeginMouBingcaiLegacyAlcoholUse(long id, long parentId, long dyingId)
    {
        if (!HasMouHuangYueyingBingcaiCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } use)
            return false;
        var first = _coreWholeActualTurnUsesOperations.WholeActualTurnUseHistory().FirstOrDefault(e => e.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic);
        if (first is null || first.OriginFrameId != id || first.CardActionId is not null)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (index < 1 || index != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != parentId || parent.OwnerSeat != use.SourceSeat || parent.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            throw new InvalidOperationException("Bingcai's legacy Alcohol requires its genuine accepted first-basic self-rescue producer.");
        use = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(use, parentId);
        if (use.Action is null)
            throw new InvalidOperationException("The opted-in first-basic Alcohol lost its native action normalization.");
        var source = new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId);
        var receipt = new MouBingcaiLegacyAlcoholReturn(id, parentId, dyingId, parent.InstructionIndex, source, parent.GameplayHash, use.Action.ActionId, MouBingcaiLegacyAlcoholStage.Targets);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouBingcaiLegacyAlcohol = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiLegacyAlcoholIssuedEvent(id, parentId, dyingId, receipt.InstructionIndex, source, receipt.ProducerHash, receipt.ActionId));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, use.Action, SkillProgramTriggerWindow.CardUseTargetsFinalized, [use.SourceSeat], ProgramCardContinuation.FinalizedSimpleCard))
            ContinueMouBingcaiLegacyAlcohol(id);
        return true;
    }

    internal bool IsMouBingcaiLegacyAlcoholUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, MouBingcaiLegacyAlcohol: { } r, Action: { Type: CardActionType.Use } a } use || r.UseId != id || a.ActionId != r.ActionId || !Enum.IsDefined(r.Stage) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || !use.TargetSeats.SequenceEqual([use.SourceSeat]) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame parent && parent.Id == r.ProgramParentId && parent.InstructionIndex == r.InstructionIndex && parent.GameplayHash == r.ProducerHash && parent.WindowContext?.ParentFrameId == r.DyingId && parent.OwnerSeat == use.SourceSeat && r.ProducerSource == new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op == SkillProgramEffectOp.UseVirtualDyingAlcohol && _coreProgramEventHistoryOperations.ProgramEventHistory<MouBingcaiLegacyAlcoholIssuedEvent>().Count(e => e == new MouBingcaiLegacyAlcoholIssuedEvent(id, r.ProgramParentId, r.DyingId, r.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId)) == 1;
    }

    [NativeUseValidation(28961)]
    internal void ValidateMouBingcaiLegacyAlcohol(CardUseFrame use)
    {
        if (use.MouBingcaiLegacyAlcohol is not null && !IsMouBingcaiLegacyAlcoholUse(use.Id))
            throw new InvalidOperationException("Bingcai lost its native legacy Alcohol producer and typed return.");
    }

    internal void ContinueMouBingcaiLegacyAlcohol(long id)
    {
        if (!IsMouBingcaiLegacyAlcoholUse(id) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Bingcai's legacy Alcohol cannot pass unfinished native children.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = use.MouBingcaiLegacyAlcohol!;
        if (r.Stage == MouBingcaiLegacyAlcoholStage.Targets)
        {
            r = r with
            {
                Stage = MouBingcaiLegacyAlcoholStage.Committed
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouBingcaiLegacyAlcohol = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, use.Action!, SkillProgramTriggerWindow.CardUseCommitted, [use.SourceSeat], ProgramCardContinuation.CommittedSimpleCard))
                return;
        }

        r = r with
        {
            Stage = MouBingcaiLegacyAlcoholStage.Recovery
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouBingcaiLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(id, r.ProgramParentId, r.DyingId);
    }

    internal bool TryReturnMouBingcaiLegacyAlcohol(ProgramCardTriggerWindowFrame window)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.MouBingcaiLegacyAlcohol is not { } r)
            return false;
        if (!IsMouBingcaiLegacyAlcoholUse(window.ParentFrameId) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.ParentFrameId || window.SimpleContinuation is not null || window.AttackOwnerFrameId is not null || window.CompletedResponseReturn is not null || window.Action.ActionId != r.ActionId || r.Stage == MouBingcaiLegacyAlcoholStage.Recovery || window.Continuation != (r.Stage == MouBingcaiLegacyAlcoholStage.Targets ? ProgramCardContinuation.FinalizedSimpleCard : ProgramCardContinuation.CommittedSimpleCard))
            throw new InvalidOperationException("Bingcai's legacy Alcohol cannot replay a native target or committed window.");
        ContinueMouBingcaiLegacyAlcohol(window.ParentFrameId);
        return true;
    }

    internal bool IsMouBingcaiLegacyDyingParent(ProgramSkillFrame root)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || root.WindowContext is not { } context)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (rootIndex < 2 || _resolutionStackOperations._resolutionStack[rootIndex - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _resolutionStackOperations._resolutionStack[rootIndex - 2] is not CardUseFrame use || window.ParentFrameId != use.Id || use.Action?.ActionId != window.Action.ActionId)
            return false;
        return use.MouBingcaiLegacyAlcohol is { } own && own.DyingId == dying.FrameId && own.Stage != MouBingcaiLegacyAlcoholStage.Recovery && IsMouBingcaiLegacyAlcoholUse(use.Id) || use.LiuYanLegacyAlcohol is { } liu && liu.DyingId == dying.FrameId && liu.Stage != LiuYanLegacyAlcoholStage.Recovery && _characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(use.Id) || use.CuiYanLegacyAlcohol is { } cui && cui.DyingId == dying.FrameId && cui.Stage != CuiYanLegacyAlcoholStage.Recovery && _characterCuiYanOlOperations.IsCuiYanLegacyAlcoholUse(use.Id) || use.ZhuJunLegacyAlcohol is { Returned: false } zhu && zhu.DyingId == dying.FrameId && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunLegacyAlcoholWindowIssuedEvent>().Count(e => e.Return == zhu) == 1;
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 670)]
    internal bool IsMouBingcaiLegacyDyingWindow()
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramCardTriggerWindowFrame window || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { } use || use.Action?.ActionId != window.Action.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !_coreProgramLifecycleOperations.GetProgramTrigger(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouHuangYueyingBingcai) || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) >= _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id))
            return false;
        return use.MouBingcaiLegacyAlcohol is { } own && own.DyingId == dying.FrameId && own.Stage != MouBingcaiLegacyAlcoholStage.Recovery && IsMouBingcaiLegacyAlcoholUse(use.Id) || use.LiuYanLegacyAlcohol is { } liu && liu.DyingId == dying.FrameId && liu.Stage != LiuYanLegacyAlcoholStage.Recovery && _characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(use.Id) || use.CuiYanLegacyAlcohol is { } cui && cui.DyingId == dying.FrameId && cui.Stage != CuiYanLegacyAlcoholStage.Recovery && _characterCuiYanOlOperations.IsCuiYanLegacyAlcoholUse(use.Id) || use.ZhuJunLegacyAlcohol is { Returned: false } zhu && zhu.DyingId == dying.FrameId && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunLegacyAlcoholWindowIssuedEvent>().Count(e => e.Return == zhu) == 1;
    }
}
