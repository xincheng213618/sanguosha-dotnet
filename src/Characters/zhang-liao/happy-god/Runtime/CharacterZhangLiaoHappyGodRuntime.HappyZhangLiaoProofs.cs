#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoHappyGodRuntime
{
    internal bool HappyZlCannotRespond(CardActionContext action, int responderSeat) => HasHappyZl && (action.Type == CardActionType.Use || action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Nullification) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(responderSeat) && _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[responderSeat].IsAlive && HappyZlColor(action.ActorSeat, responderSeat, action.EffectiveIsRed ?? global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(action.EffectiveSuit))is not null;
    internal bool HappyZlCannotRespond(long useId, int responderSeat) => HasHappyZl && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } use && action.Type == CardActionType.Use && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind && HappyZlCannotRespond(action, responderSeat);
    internal bool HappyZlNullificationForbidden(NullificationWindowFrame window, int responderSeat)
    {
        if (!HasHappyZl || !_coreDelayedNullificationOperations.HasNullificationEffectOwner(window))
            return false;
        if (window.ChainDepth == 0)
            return window.DelayedReturn is null && HappyZlCannotRespond(window.ParentFrameId, responderSeat);
        if (window.HappyZhangLiao is not { } r || r.Window != window.Id || r.Parent != window.ParentFrameId || r.Depth != window.ChainDepth || HappyZlColor(r.Actor, r.Target, r.Color)is not { } grant || grant.FrameId != r.GrantFrame)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlCounterspellEvent>().Count(e => e.Receipt == r) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == r.ActionId && e.Action.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(window) && e.Action.ActorSeat == r.Actor && e.Action.Type == CardActionType.Response && e.Action.EffectiveKind == CardKind.Nullification && HappyZlCannotRespond(e.Action, responderSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 40980)]
    internal void ObserveHappyZlCounterspell(IGameEvent payload)
    {
        if (!HasHappyZl || payload is not CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } a } || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } w || w.ChainDepth < 1 || !_coreDelayedNullificationOperations.HasNullificationEffectOwner(w) || a.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(w) || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null || a.OpponentSeat != w.SourceSeat)
            return;
        var grant = _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlColorEvent>().LastOrDefault(e => e.Source.OwnerSeat == a.ActorSeat && HappyZlCannotRespond(a, e.Target));
        if (grant?.Color is not { } color)
            return;
        var receipt = new HappyZlCounterspell(w.Id, w.ParentFrameId, w.ChainDepth, a.ActionId, a.ActorSeat, grant.Target, color, grant.FrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { HappyZhangLiao = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlCounterspellEvent(receipt));
    }
    internal bool HappyZlMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.HappyZhangLiao is not { } r || e?.Op != r.Op || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || r.Stage is not (HappyZlStage.GainChildren or HappyZlStage.SlotChildren or HappyZlStage.RestoreChildren or HappyZlStage.DrawChildren or HappyZlStage.DiscardChildren))
            return false;
        return r.Payments.Count > 0 && (r.Stage != HappyZlStage.DrawChildren || r.Draw is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == r.Restored);
    }

    internal bool HappyZlFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HappyZhangLiao is not { } r || f.PendingMovementContinuation is not { } p || !HappyZlMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return false;
        var moves = r.Payments.Last().Moves;
        if (child is CardsMovedTriggerWindowFrame m)
            return m.Batch.ParentFrameId == f.Id && m.Batch.OriginOwnerSeat == f.OwnerSeat && m.Batch.OriginSkillId == f.SkillId && m.Batch.OriginSkillInstanceId == f.SkillInstanceId && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, m) && m.Batch.Movements.All(x => moves.Contains(x));
        if (child is RecoveryReplacementFrame recovery)
            return moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == recovery.Attempt.TargetSeat) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == hp.Change.TargetSeat);
        return false;
    }

    internal ProgramSkillFrame? HappyZlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HappyZlFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40980)]
    internal bool HappyZlDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HappyZlObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40980)]
    internal bool AdvanceHappyZlChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyZlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40980)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40980)]
    internal bool RetainHappyZlPaid(ProgramSkillFrame f)
    {
        if (f.HappyZhangLiao is not { Payments.Count: > 0 })
            return false;
        return true;
    }
}
