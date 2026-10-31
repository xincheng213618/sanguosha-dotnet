#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal void ResolveHuanpeiDodge(HuanpeiPaidOrigin p)
    {
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Huanpei lost its original incoming Slash.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        if (!ValidHuanpeiPayment(p) || p.Request.Intent != HuanpeiIntent.OwnSlashDodge || p.Request.ParentFrameId != use.Id || p.Request.ParentActionId != use.Action?.ActionId || p.Request.Cursor != attack.SuccessfulDodgeResponses || attack.TargetSeat != actor.Seat || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge))
            throw new InvalidOperationException("Huanpei cannot replace a pure Played or another native Dodge request.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(use.Id);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var a = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, use.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [p.Source], effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { HuanpeiResponse = new(p, use.Id, a.ActionId) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiIssuedEvent(p.ProgramFrameId, use.Id, a.ActionId, actor.Seat, CardKind.Dodge, p.Request.Intent));
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Dodge);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(a);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, attack.SourceSeat, CardKind.Dodge));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(a));
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(attack, a, ProgramCardContinuation.Dodge))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, a, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.Dodge))
            return;
        if (_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(a, ProgramCardContinuation.Dodge) || _coreResponseCompletionOperations.HasCardResponseCompletedObserver(a, ProgramCardContinuation.Dodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, a, ProgramCardContinuation.Dodge);
        else
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal CardActionContext RequireHuanpeiResponseAction(CardUseFrame use)
    {
        var r = use.HuanpeiResponse ?? throw new InvalidOperationException("Huanpei's own Dodge has no issued receipt.");
        var p = r.Payment;
        var accepted = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == r.CardActionId).ToArray();
        if (!ValidHuanpeiPayment(p) || r.OwnerFrameId != use.Id || p.Kind != CardKind.Dodge || p.Request.Intent != HuanpeiIntent.OwnSlashDodge || use.CardAttack is not { } attack || attack.TargetSeat != p.Source.OwnerSeat || attack.SuccessfulDodgeResponses != p.Request.Cursor || p.Request.ParentFrameId != use.Id || p.Request.ParentActionId != use.Action?.ActionId || accepted is not [var fact] || fact.Action is not { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, PhysicalCards.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: null } a || a.ActorSeat != p.Source.OwnerSeat || a.ProviderSeat != a.ActorSeat || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null || a.OpponentSeat != attack.SourceSeat || a.ConversionChain is not [var source] || source != p.Source || a.ParentActionId != use.Action?.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiIssuedEvent>().Count(e => e.PaymentFrameId == p.ProgramFrameId && e.ActionId == a.ActionId && e.UseId == use.Id && e.Intent == p.Request.Intent) != 1)
            throw new InvalidOperationException("Huanpei's true Dodge changed its accepted action or original native cursor.");
        return a;
    }

    internal void ClearHuanpeiDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { HuanpeiResponse: { } r } use)
            return;
        var a = RequireHuanpeiResponseAction(use);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == a.ActionId) || use.RecoveryPaidContinuation is not null || use.PendingRecoveryAttempts is { Count: > 0 })
            throw new InvalidOperationException("Huanpei cannot clear its native Dodge while a child remains.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiReturnedEvent(r.Payment.ProgramFrameId, use.Id, a.ActionId));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { HuanpeiResponse = null });
    }

    internal bool HasHuanpeiNativeRequestContinuation(long parentId) => HuanpeiActiveDraft()is { HuanShuJun.Request: { } n } f && n.ParentFrameId == parentId && HuanpeiRequestParentCurrent(n) && HuanShuJunPaidOrPreparing(f) && (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || HuanShuJunObserverRoot()?.Id == f.Id);
    internal void AssertHuanpeiRequests()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.HuanShuJun?.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse))
        {
            AssertHuanShuJun(f);
            if (f.HuanShuJun!.Request is not { } n || n.Intent != HuanpeiIntent.Play && (!HuanpeiRequestParentCurrent(n) || f.HuanShuJun.OriginalDecision is not { } original || original.PromptId != n.OriginalPromptId || original.Revision != n.OriginalRevision))
                throw new InvalidOperationException("Huanpei lost its frozen genuine native request.");
        }
    }

    internal bool HuanpeiDyingRescueRide(int index, DyingFrame dying)
    {
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] != dying || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dying.Id)
            return false;
        if (_resolutionStackOperations._resolutionStack[index + 1] is ProgramSkillFrame { HuanShuJun: { Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, Request: { Intent: HuanpeiIntent.Dying } n } } preparation)
            return n.ParentFrameId == dying.Id && n.RequestFrameId == dying.Id && n.ActorSeat == dying.ResponderSeat && n.Cursor == dying.ResponderIndex && n.FixedTargetSeat == dying.VictimSeat && HuanShuJunPaidOrPreparing(preparation) && HuanShuJunObserverSuffix(index + 1);
        if (_resolutionStackOperations._resolutionStack[index + 1] is not CardUseFrame { HuanpeiUse: { } receipt } use || !IsIssuedHuanpeiUse(use) || receipt.Payment.Request.Intent != HuanpeiIntent.Dying || receipt.Payment.Request.ParentFrameId != dying.Id || receipt.Payment.Request.Cursor != dying.ResponderIndex || use.SourceSeat != dying.ResponderSeat || use.DyingResponse is not { } token || token.ResolutionId != dying.Id || !use.TargetSeats.SequenceEqual([dying.VictimSeat]) || token.ResponderSeat != dying.ResponderSeat)
            return false;
        for (var i = index + 2; i < _resolutionStackOperations._resolutionStack.Count; i++)
            if (!_coreParticipantHandDamageRidesOperations.ParticipantHandRescueObserverRide(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1], use))
                return false;
        return true;
    }

    internal bool HasHuanpeiForcedSlash(CharacterState actor, CharacterState target, global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.ActiveAttack is null && pending.WeaponOwnerSeat == actor.Seat && pending.SlashTargetSeat == target.Seat && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && HasHuanpeiForcedSlash(actor, target, false);
    internal bool HasHuanpeiForcedSlash(CharacterState actor, CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.Attack.ResolutionId)is { } use && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && pending.Attack.SourceSeat == actor.Seat && pending.Attack.TargetSeat == target.Seat && HasHuanpeiForcedSlash(actor, target, true);
    internal bool HuanpeiPausedRequestRide(ProgramSkillFrame f, long owner)
    {
        if (f.HuanShuJun is not { Paid: true, Request: { } n, OriginalDecision: { } original } r || n.ParentFrameId != owner || !HuanpeiRequestParentCurrent(n) || !HuanShuJunPaidOrPreparing(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiAdjustmentAcceptedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Round == r.Round) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiRequestCapturedEvent>().Count(e => e.FrameId == f.Id && e.Request == n && e.DecisionHash == HuanShuJunDecisionHash(original)) != 1)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != n.RequestFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || HuanShuJunObserverRoot()?.Id == f.Id;
    }

    internal bool HasHuanpeiBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => HuanpeiActiveDraft()is { HuanShuJun: { Request: { Intent: HuanpeiIntent.BorrowedSword } n, OriginalDecision: { Kind: DecisionKind.RespondSlash } original } } f && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) && pending.ActiveAttack is null && pending.AwaitingSlashChoice && pending.WeaponOwnerSeat == f.OwnerSeat && pending.SlashTargetSeat == n.FixedTargetSeat && n.Cursor == 0 && original.PlayerSeat == f.OwnerSeat && original.TargetSeat == pending.SlashTargetSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { Action: { } action } && n.ParentActionId == action.ActionId && HuanpeiPausedRequestRide(f, pending.ResolutionId);
    internal bool HasHuanpeiQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => HuanpeiActiveDraft()is { HuanShuJun: { Request: { Intent: HuanpeiIntent.Qinglong } n, OriginalDecision: { Kind: DecisionKind.QinglongCrescentBlade } original } } f && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } blade && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(blade.Attack, attack) && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == n.FixedTargetSeat && n.Cursor == 0 && original.PlayerSeat == f.OwnerSeat && original.TargetSeat == attack.TargetSeat && original.IncomingCard == attack.EffectiveCardKind && original.ValidTargetSeats.SequenceEqual([attack.TargetSeat]) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Action: { } action } && n.ParentActionId == action.ActionId && HuanpeiPausedRequestRide(f, attack.ResolutionId);
}
