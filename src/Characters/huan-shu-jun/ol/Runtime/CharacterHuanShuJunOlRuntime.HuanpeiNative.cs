#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal bool ValidHuanpeiPayment(HuanpeiPaidOrigin p)
    {
        if (p.Source.SkillId != "ol:huanpei" || p.Source.BindingId != "adjust-and-use-basic" || p.Source.OwnerSeat != p.Request.ActorSeat || p.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash || !global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlDefinitions.HuanpeiKinds.Contains(p.Kind) || p.SequenceBefore < 0 || p.SequenceAfter < p.SequenceBefore || p.Targets is not System.Collections.IList { IsReadOnly: true })
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HuanShuJunStartedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiAdjustmentAcceptedEvent>().Where(e => e.FrameId == p.ProgramFrameId).ToArray()is [var paid] && paid.OwnerSeat == p.Source.OwnerSeat && paid.Round == p.Round && paid.HandCount == p.FrozenHandCount && paid.Hp == p.FrozenHp && paid.RequestedCount == p.AdjustmentCount && paid.Before == p.SequenceBefore && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiAdjustmentCompletedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.Before == p.SequenceBefore && e.After == p.SequenceAfter) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiOutputChosenEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.Kind == p.Kind && e.TargetCount == p.Targets.Count) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiTargetChosenEvent>().Where(e => e.FrameId == p.ProgramFrameId).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(p.Targets);
    }

    internal static bool SameHuanpeiPayment(HuanpeiPaidOrigin a, HuanpeiPaidOrigin b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_huan_shu_jun_ol_HuanpeiNative.SameHuanpeiPayment(a, b);
    internal CardActionContext PrepareHuanpeiAction(CardActionContext a, HuanpeiPaidOrigin? p) => p is null ? a : _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, a.PhysicalCards, a.ConversionChain, a.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
    internal void IssueHuanpeiUse(CardUseFrame use, HuanpeiPaidOrigin? p)
    {
        if (p is null)
            return;
        if (!ValidHuanpeiPayment(p) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: null } a || a.ActorSeat != p.Source.OwnerSeat || a.ProviderSeat != a.ActorSeat || a.RequesterSeat is not null || a.EffectiveKind != p.Kind || a.ConversionChain is not [var source] || source != p.Source || _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiIssuedEvent>().Any(e => e.PaymentFrameId == p.ProgramFrameId))
            throw new InvalidOperationException("Huanpei requires its exact paid, zero-material native Use.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { HuanpeiUse = new(p, use.Id, a.ActionId) });
        if (p.Request.ParentFrameId is { } parentId && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == parentId)is ProgramSkillFrame parent)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HuanpeiReturn = new(p, use.Id, a.ActionId) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiIssuedEvent(p.ProgramFrameId, use.Id, a.ActionId, a.ActorSeat, p.Kind, p.Request.Intent));
    }

    internal bool IsIssuedHuanpeiUse(CardUseFrame use)
    {
        if (use.HuanpeiUse is not { } r || r.OwnerFrameId != use.Id || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use } current || !ValidHuanpeiPayment(r.Payment))
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }

        var p = r.Payment;
        return a.ActionId == r.CardActionId && a.ProviderSeat == p.Source.OwnerSeat && a.RequesterSeat is null && a.PhysicalCards.Count == 0 && a.ConversionChain is [var source] && source == p.Source && a.EffectiveKind == p.Kind && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.EffectiveIsRed is null && (a.ActorSeat == p.Source.OwnerSeat && use.SourceSeat == a.ActorSeat || _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, a.ActorSeat, p.Source.OwnerSeat)) && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiIssuedEvent>().Count(e => e.PaymentFrameId == p.ProgramFrameId && e.UseId == use.Id && e.ActionId == a.ActionId && e.ActorSeat == p.Source.OwnerSeat && e.Kind == p.Kind && e.Intent == p.Request.Intent) == 1;
    }

    internal bool IsHuanpeiUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedHuanpeiUse(use);
    internal Card HuanpeiRepresentation(long id) => IsHuanpeiUse(id) ? new(0, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind, Suit.None, 0) : throw new InvalidOperationException("Huanpei's neutral native representation lost its paid Use.");
    internal bool SkipHuanpeiFinishedMovement(long id, Card c) => c.Id == 0 && IsHuanpeiUse(id) && c.Kind == _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind;
    internal bool IsHuanpeiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { HuanpeiUse: { } r } use || !IsIssuedHuanpeiUse(use) || attack.Card is not null || attack.PhysicalCards.Count != 0 || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.SourceSeat != use.SourceSeat || attack.EffectiveCardKind != use.CardKind)
            return false;
        var position = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        var ancestors = _resolutionStackOperations._resolutionStack.Take(position).OfType<CardUseFrame>().SelectMany(f => f.PhysicalCardIds ?? []).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Distinct().Order().ToArray();
        return processing.Select(c => c.Id).Distinct().Order().SequenceEqual(ancestors) && (r.Payment.Request.ParentSkillId is null || attack.ProgramSkillCardUseFrameId == r.Payment.Request.ParentFrameId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Payment.Request.ParentFrameId)is ProgramSkillFrame p && HasHuanpeiProgramPayment(p, use));
    }

    internal bool ContinueHuanpeiSimpleUse(long id, ProgramSimpleCardContinuation c)
    {
        if (c.CardId != 0 || !IsHuanpeiUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var owner = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = HuanpeiRepresentation(id);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, use.CardKind);
            return true;
        }

        if (c.Effect == SimpleCardUseEffect.Recovery && use.CardKind is CardKind.Peach or CardKind.Alcohol)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(owner, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, id, use.CardKind, c.RecoveryAmount, c.RecoveryPolicySources ?? []);
        else if (c.Effect == SimpleCardUseEffect.Alcohol && use.CardKind == CardKind.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(owner, card, id);
        else
            throw new InvalidOperationException("Huanpei changed its accepted native basic effect.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 40)]
    internal bool FinishHuanpeiAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsHuanpeiUse(attack.ResolutionId) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(28100)]
    internal void ValidateHuanpeiUse(CardUseFrame use)
    {
        if (use.HuanpeiUse is not null && (!IsIssuedHuanpeiUse(use) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("Huanpei cannot return before its whole native Use and children.");
    }

    [NativeUseReturn(17)]
    internal void ReturnHuanpeiUse(CardUseFrame use)
    {
        if (use.HuanpeiUse is not { } r)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiReturnedEvent>().Any(e => e.PaymentFrameId == r.Payment.ProgramFrameId))
            throw new InvalidOperationException("Huanpei can return only once after the real native Use.");
        if (r.Payment.Request.ParentFrameId is { } id && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is ProgramSkillFrame { HuanpeiReturn: { } lease } p)
        {
            if (!SameHuanpeiPayment(lease.Payment, r.Payment) || lease.UseId != use.Id || lease.ActionId != r.CardActionId)
                throw new InvalidOperationException("Huanpei returned to another native program request.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(p.Id, p with { HuanpeiReturn = lease with { Returned = true } });
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiReturnedEvent(r.Payment.ProgramFrameId, use.Id, r.CardActionId));
        if (r.Payment.Request.ParentFrameId is { } genericId && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == genericId)is ProgramSkillFrame generic && r.Payment.Request.ParentOperation is SkillProgramEffectOp.RequestSlashByTarget or SkillProgramEffectOp.RequestSlashByNearest or SkillProgramEffectOp.RequestSlashAgainstChosenTarget or SkillProgramEffectOp.RequestLegalSlashByNearest or SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
        {
            if (!HasHuanpeiProgramPayment(generic) || generic.HuanpeiReturn?.Returned != true)
                throw new InvalidOperationException("Huanpei's generic native request has not actually returned.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(generic.Id, generic with { HuanpeiReturn = null });
        }
    }

    internal bool HasHuanpeiProgramPayment(ProgramSkillFrame f, CardUseFrame? use = null)
    {
        if (f.HuanpeiReturn is not { } r || !ValidHuanpeiPayment(r.Payment) || r.Payment.Request is not { ParentFrameId: { } parent, RequestFrameId: { } request } n || parent != f.Id || request != f.Id || n.Cursor != f.InstructionIndex || n.ParentSkillId != f.SkillId || n.ParentSkillInstanceId != f.SkillInstanceId || n.ParentGameplayHash != f.GameplayHash || n.ParentOperation != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op)
            return false;
        if (use is not null && (use.HuanpeiUse is not { } u || !SameHuanpeiPayment(u.Payment, r.Payment) || u.OwnerFrameId != r.UseId || u.CardActionId != r.ActionId || !IsIssuedHuanpeiUse(use)))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiIssuedEvent>().Count(e => e.PaymentFrameId == r.Payment.ProgramFrameId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiReturnedEvent>().Count(e => e.PaymentFrameId == r.Payment.ProgramFrameId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1);
    }

    internal void ExecuteHuanpeiPaid(HuanpeiPaidOrigin p)
    {
        if (!ValidHuanpeiPayment(p) || !_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive || !HuanpeiRequestParentCurrent(p.Request))
            return;
        PrepareHuanpeiOriginalParent(p);
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        var kind = p.Kind;
        var card = new Card(0, kind, Suit.None, 0);
        if (p.Request.Intent == HuanpeiIntent.OwnSlashDodge)
        {
            ResolveHuanpeiDodge(p);
            return;
        }

        if (p.Request.Intent == HuanpeiIntent.Dying)
        {
            var d = _coreDyingCursorRuntimeOperations.ActiveDying!;
            var policies = kind == CardKind.Peach && actor.Seat != d.VictimSeat ? _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[d.VictimSeat], SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(policy => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(actor), policy.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
            _coreNativeDyingFramesOperations.SetDyingFrameStep(d.FrameId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, _coreMatchParticipantsPlayersAccess._players[d.VictimSeat], card, kind == CardKind.Peach ? "桃" : "酒", kind, 1 + policies.Sum(policy => policy.Policy.Value), recoveryPolicySources: policies.Select(policy => (policy.Source.SkillId, policy.Policy.Id)).ToArray(), conversionSource: p.Source, dyingResponse: new(d.FrameId, actor.Seat, kind == CardKind.Peach, kind == CardKind.Peach ? 0 : null, kind == CardKind.Alcohol, kind == CardKind.Alcohol ? 0 : null), physicalCards: [], huanpeiOrigin: p);
            return;
        }

        if (p.Request.Intent == HuanpeiIntent.WenyiPeach)
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(p.Request.ParentFrameId!.Value);
            if (!parent.HuanShuJun!.Paid)
                PrepareWenyiSuppliedPeach(parent, actor.Seat, [], p.Source);
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, _coreMatchParticipantsPlayersAccess._players[p.Request.FixedTargetSeat!.Value], card, "桃", CardKind.Peach, conversionSource: p.Source, physicalCards: [], huanpeiOrigin: p);
            return;
        }

        if (p.Request.Intent is HuanpeiIntent.Play or HuanpeiIntent.KeBiNengBasic)
        {
            if (kind is CardKind.Peach or CardKind.Alcohol && p.Targets.Count > 1)
                _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
            if (kind == CardKind.Peach)
            {
                _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, card, "桃", kind, conversionSource: p.Source, physicalCards: [], huanpeiOrigin: p);
                return;
            }

            if (kind == CardKind.Alcohol)
            {
                var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [], kind, physicalCardIds: [], conversionSource: p.Source, huanpeiOrigin: p);
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
                return;
            }

            BeginHuanpeiSlash(p, p.Request.Intent == HuanpeiIntent.Play, null);
            return;
        }

        if (p.Request.Intent == HuanpeiIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(b.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            b.AwaitingSlashChoice = false;
            b.SlashCardId = 0;
            b.EffectiveSlashKind = kind;
            BeginHuanpeiSlash(p, true, b);
            return;
        }

        if (p.Request.Intent == HuanpeiIntent.Qinglong)
        {
            var outer = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(outer.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(outer.ResolutionId, actor.Seat, p.Request.FixedTargetSeat!.Value, true, [], kind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(outer.ResolutionId);
            BeginHuanpeiSlash(p, false, null);
            return;
        }

        var parentProgram = _coreProgramLifecycleOperations.GetActiveProgramFrame(p.Request.ParentFrameId!.Value);
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parentProgram)!;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(parentProgram))
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(parentProgram, actor.Seat, actor.Seat, p.Targets.Single(), kind, [], p.Source);
        else if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parentProgram))
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(parentProgram, actor.Seat, actor.Seat, p.Targets.Single(), kind, [], p.Source);
        else if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parentProgram) || _characterWuJingOlOperations.IsWuJingHejiRequest(parentProgram) || parentProgram.CaoChun?.Stage == CaoChunStage.NativeSlash)
        {
        }
        else if (p.Request.Intent == HuanpeiIntent.ProgramSlash)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parentProgram.Id, effect.ResultBind!, "used-slash", actor.Seat, "环佩：视为使用杀。");
        else if (effect.Op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parentProgram = parentProgram with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parentProgram.Id, effect.ResultBind!, "used-slash", actor.Seat, "环佩：视为使用杀。");
        }
        else if (effect.Op == SkillProgramEffectOp.RequestLegalSlashByNearest)
            _coreRuntimeOperations.ReplaceRuntimeTop(parentProgram with { NearestLegalSlashRequest = null });
        else if (p.Request.Intent == HuanpeiIntent.FixedSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parentProgram with { FixedTargetSlash = parentProgram.FixedTargetSlash!with { Issued = true } });
        BeginHuanpeiSlash(p, false, null);
    }

    internal void BeginHuanpeiSlash(HuanpeiPaidOrigin p, bool counts, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        var card = new Card(0, p.Kind, Suit.None, 0);
        var program = p.Request.ParentSkillId is null ? (long? )null : p.Request.ParentFrameId;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, p.Targets, p.Kind, physicalCardIds: [], conversionSource: p.Source, huanpeiOrigin: p);
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], p.Kind), programSkillCardUseFrameId: program);
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (counts && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, p.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
}
