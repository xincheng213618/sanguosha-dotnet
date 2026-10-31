#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    internal bool IsCuiYanLegacyAlcoholUse(long id)
    {
        if (!HasCuiYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, CuiYanLegacyAlcohol: { } r, Action: { Type: CardActionType.Use } a } u || r.UseId != id || a.ActionId != r.ActionId || !Enum.IsDefined(r.Stage) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(u) || !u.TargetSeats.SequenceEqual([u.SourceSeat]) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != r.ProgramParentId || p.InstructionIndex != r.InstructionIndex || p.GameplayHash != r.ProducerHash || p.WindowContext?.ParentFrameId != r.DyingId || p.OwnerSeat != u.SourceSeat || r.ProducerSource != new CardConversionSource(p.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)?.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanLegacyAlcoholWindowIssuedEvent>().Count(e => e == new CuiYanLegacyAlcoholWindowIssuedEvent(id, r.ProgramParentId, r.DyingId, r.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId)) != 1)
            return false;
        return true;
    }

    [NativeUseValidation(27471)]
    internal void ValidateCuiYanLegacyAlcohol(CardUseFrame use)
    {
        if (use.CuiYanLegacyAlcohol is not null && !IsCuiYanLegacyAlcoholUse(use.Id))
            throw new InvalidOperationException("Shutong's legacy Wine lost its exact native producer and return.");
    }

    internal bool TryBeginCuiYanLegacyAlcoholUse(long id, long parentId, long dyingId)
    {
        if (!HasCuiYanCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: null, CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } u || !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse)))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (i < 1 || i != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame p || p.Id != parentId || p.OwnerSeat != u.SourceSeat || p.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dyingId || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(u))
            throw new InvalidOperationException("Shutong's legacy Wine requires its genuine accepted self-rescue producer.");
        u = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(u, parentId);
        var r = new CuiYanLegacyAlcoholReturn(id, parentId, dyingId, p.InstructionIndex, new(p.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId), p.GameplayHash, u.Action!.ActionId, CuiYanLegacyAlcoholStage.Targets);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholWindowIssuedEvent(id, parentId, dyingId, p.InstructionIndex, r.ProducerSource, r.ProducerHash, r.ActionId));
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action, SkillProgramTriggerWindow.CardUseTargetsFinalized, [u.SourceSeat], ProgramCardContinuation.FinalizedSimpleCard))
            return true;
        ContinueCuiYanLegacyAlcohol(id);
        return true;
    }

    internal void ContinueCuiYanLegacyAlcohol(long id)
    {
        if (!IsCuiYanLegacyAlcoholUse(id) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Shutong's legacy Wine lost its exact native target/commit return.");
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = u.CuiYanLegacyAlcohol!;
        if (r.Stage == CuiYanLegacyAlcoholStage.Targets)
        {
            r = r with
            {
                Stage = CuiYanLegacyAlcoholStage.Committed
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, u.Action!, SkillProgramTriggerWindow.CardUseCommitted, [u.SourceSeat], ProgramCardContinuation.CommittedSimpleCard))
                return;
        }

        r = r with
        {
            Stage = CuiYanLegacyAlcoholStage.Recovery
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { CuiYanLegacyAlcohol = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanLegacyAlcoholStageEvent(id, r.ActionId, r.Stage));
        _coreDyingProgramsOperations.ResumeProgramVirtualDyingAlcoholRecovery(id, r.ProgramParentId, r.DyingId);
    }

    internal bool TryReturnCuiYanLegacyAlcohol(ProgramCardTriggerWindowFrame w)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)?.CuiYanLegacyAlcohol is not { } r)
            return false;
        if (!IsCuiYanLegacyAlcoholUse(w.ParentFrameId) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != w.ParentFrameId || w.SimpleContinuation is not null || w.AttackOwnerFrameId is not null || w.CompletedResponseReturn is not null || w.Action.ActionId != r.ActionId || w.Continuation != (r.Stage == CuiYanLegacyAlcoholStage.Targets ? ProgramCardContinuation.FinalizedSimpleCard : ProgramCardContinuation.CommittedSimpleCard) || r.Stage == CuiYanLegacyAlcoholStage.Recovery)
            throw new InvalidOperationException("Shutong's legacy Wine cannot replay a target or commit return.");
        ContinueCuiYanLegacyAlcohol(w.ParentFrameId);
        return true;
    }

    internal bool IsCuiYanOriginalDyingRide()
    {
        if (!HasCuiYanCapability || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying)
            return false;
        var di = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId);
        if (di < 0)
            return false;
        foreach (var u in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            var ui = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == u.Id);
            if (ui <= di)
                continue;
            var physical = u.DyingResponse is { } response && response.ResolutionId == dying.FrameId && response.ResponderSeat == u.SourceSeat && u.CardKind is CardKind.Peach or CardKind.Alcohol && u.TargetSeats.SequenceEqual([dying.VictimSeat]);
            var legacy = u.CuiYanLegacyAlcohol is { } r && r.DyingId == dying.FrameId && r.Stage != CuiYanLegacyAlcoholStage.Recovery && IsCuiYanLegacyAlcoholUse(u.Id);
            var otherLegacy = u.LiuYanLegacyAlcohol is { } lr && lr.DyingId == dying.FrameId && lr.Stage != LiuYanLegacyAlcoholStage.Recovery && _characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(u.Id);
            if (!physical && !legacy && !otherLegacy)
                continue;
            if (ui + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[ui + 1] is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != u.Id || w.Action.ActionId != u.Action?.ActionId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count)
                continue;
            var candidate = w.Candidates[w.CandidateIndex];
            if (ui + 2 == _resolutionStackOperations._resolutionStack.Count)
                return legacy && _coreProgramLifecycleOperations.GetProgramTrigger2(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShutongFinalizedUse);
            if (_resolutionStackOperations._resolutionStack[ui + 2] is not ProgramSkillFrame binding || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(binding, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)) || binding.WindowContext?.ParentFrameId != w.Id)
                continue;
            if (binding.CuiYan is not null)
            {
                AssertCuiYan(binding);
                if (ui + 3 == _resolutionStackOperations._resolutionStack.Count || CuiYanObserverRoot()?.Id == binding.Id)
                    return true;
            }

            if (legacy && binding.LiuYan is not null && _characterLiuYanOlOperations.ValidLiuYan(binding) && (ui + 3 == _resolutionStackOperations._resolutionStack.Count || _characterLiuYanOlOperations.LiuYanObserverRoot()?.Id == binding.Id))
                return true;
            if (legacy && binding.ZhuJunCuipoDraw is not null && (ui + 3 == _resolutionStackOperations._resolutionStack.Count || _characterZhuJunOlOperations.ZhuJunObserverRoot()?.Id == binding.Id))
                return true;
        }

        return false;
    }
}
