#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicUsesRuntime
{
    internal void ExecuteDrawFundedDistinctBasicPaid(DrawFundedDistinctBasicFrame frame)
    {
        var p = frame.Payment;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        var kind = p.EffectiveKind;
        var card = new Card(0, kind, Suit.None, 0);
        if (p.Intent == DrawFundedDistinctBasicIntent.WenyiPeach)
        {
            _coreWenyiPeachSuppliesOperations.ExecuteWenyiDrawFundedPeach(frame);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge)
        {
            _coreDrawFundedDistinctBasicResponsesOperations.Value.ResolveDrawFundedDistinctBasicDodge(p);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.Dying)
        {
            var dying = _coreDyingCursorRuntimeOperations.Value.ActiveDying!;
            var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
            _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.ResolvingEffect);
            var policies = kind == CardKind.Peach && actor.Seat != victim.Seat ? _coreProgramCardPoliciesOperations.CardPolicies(victim, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(policy => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(actor), policy.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, victim, card, kind == CardKind.Peach ? "桃" : "酒", kind, 1 + policies.Sum(policy => policy.Policy.Value), recoveryPolicySources: policies.Select(policy => (policy.Source.SkillId, policy.Policy.Id)).ToArray(), conversionSource: p.Source, dyingResponse: new(dying.FrameId, actor.Seat, kind == CardKind.Peach, kind == CardKind.Peach ? 0 : null, kind == CardKind.Alcohol, kind == CardKind.Alcohol ? 0 : null), physicalCards: [], drawFundedPayment: p);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.Play)
        {
            // Payment resume has revalidated these exact targets against the
            // current native menu. Reuse the existing adjusted simple-use loop.
            if (kind is CardKind.Peach or CardKind.Alcohol && frame.TargetSeats.Count > 1)
                _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = frame.TargetSeats;
            if (kind == CardKind.Peach)
            {
                _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, card, "桃", kind, conversionSource: p.Source, physicalCards: [], drawFundedPayment: p);
                return;
            }

            if (kind == CardKind.Alcohol)
            {
                var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [], kind, physicalCardIds: [], conversionSource: p.Source, drawFundedPayment: p);
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
                return;
            }

            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, true, null);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.BorrowedSword)
        {
            var borrowed = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(borrowed.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(borrowed.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            borrowed.AwaitingSlashChoice = false;
            borrowed.SlashCardId = 0;
            borrowed.EffectiveSlashKind = kind;
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, borrowed, true, null);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.Qinglong)
        {
            var original = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(original.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(original.ResolutionId, actor.Seat, p.TargetSeat!.Value, Used: true, SlashCardIds: [], EffectiveSlashKind: kind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(original.ResolutionId);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, null);
            return;
        }

        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A paid program Slash lost its original request.");
        if (parent.Id != p.ParentFrameId || parent.InstructionIndex != p.Cursor)
            throw new InvalidOperationException("A paid program Slash moved its original instruction cursor.");
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent))
        {
            _characterLvKuangLvXiangOlOperations.PrepareQigongDrawFundedSlash(parent, p);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterWuJingOlOperations.IsWuJingHejiRequest(parent))
        {
            _characterWuJingOlOperations.PrepareWuJingDrawFundedSlash(parent, p);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(parent))
        {
            _characterCaoChunOlOperations.PrepareCaoChunDrawFundedSlash(parent, p);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        var effect = ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(parent))
        {
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(parent, actor.Seat, actor.Seat, p.TargetSeat!.Value, kind, [], p.Source);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parent))
        {
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(parent, actor.Seat, actor.Seat, p.TargetSeat!.Value, kind, [], p.Source);
            BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.ProgramSlash)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, effect.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, actor.Seat, "摸牌后视为使用【杀】。");
        else if (p.Intent == DrawFundedDistinctBasicIntent.AssistedSlash)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, effect.ResultBind!, "used-slash", actor.Seat, "摸牌后视为使用【杀】。");
        }
        else if (p.Intent == DrawFundedDistinctBasicIntent.NearestLegalSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NearestLegalSlashRequest = null });
        else if (p.Intent != DrawFundedDistinctBasicIntent.ProgramNearestSlash)
            throw new InvalidOperationException("A paid Slash has an unknown typed native return.");
        BeginDrawFundedDistinctBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
    }

    internal void BeginDrawFundedDistinctBasicSlash(CharacterState actor, IReadOnlyList<int> targets, DrawFundedDistinctBasicPayment p, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent)
    {
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind) || targets.Count == 0)
            throw new InvalidOperationException("A paid zero-material Slash requires its accepted ordered targets.");
        var kind = p.EffectiveKind;
        var card = new Card(0, kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind), physicalCardIds: [], conversionSource: p.Source, drawFundedPayment: p);
        var actualTargets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, actualTargets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[actualTargets[0]], kind), programSkillCardUseFrameId: programParent);
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (countsTowardLimit && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, p.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, actualTargets[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[actualTargets[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, actualTargets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal CardActionContext PrepareDrawFundedDistinctBasicAction(CardActionContext action, DrawFundedDistinctBasicPayment? payment) => payment is null ? action : _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: false));
    internal void IssueDrawFundedDistinctBasicUse(CardUseFrame use, DrawFundedDistinctBasicPayment? payment)
    {
        if (payment is not { } p)
            return;
        if (!_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p) || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || action.ActorSeat != p.Source.OwnerSeat || action.ProviderSeat != p.Source.OwnerSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.EffectiveKind != p.EffectiveKind || use.CardKind != p.EffectiveKind || action.ConversionChain is not [var source] || source != p.Source || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || action.EffectiveIsRed != false || p.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge || !_coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicNameAvailable(p.Source.OwnerSeat, p.MethodLedgerId, p.EffectiveKind))
            throw new InvalidOperationException("A draw-funded Use lost its unique paid source, neutral zero materials or actual native issuance.");
        var receipt = new DrawFundedDistinctBasicUseReceipt(p, use.Id, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { DrawFundedDistinctBasicUse = receipt });
        EmitDrawFundedDistinctBasicIssued(receipt);
    }

    internal void EmitDrawFundedDistinctBasicIssued(DrawFundedDistinctBasicUseReceipt receipt)
    {
        var p = receipt.Payment;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicIssuedEvent>().Any(e => e.PaymentFrameId == p.PaymentFrameId))
            throw new InvalidOperationException("A single paid draw attempt cannot issue two basic Uses.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawFundedDistinctBasicIssuedEvent(p.PaymentFrameId, receipt.OwnerFrameId, receipt.CardActionId, p.Source.OwnerSeat, p.MethodLedgerId, p.ActualTurnNumber, p.ActualTurnOwnerSeat, p.NormalizedName, p.EffectiveKind, p.Intent, p.Source, p.GameplayHash));
    }

    internal bool IsDrawFundedDistinctBasicIssued(DrawFundedDistinctBasicUseReceipt receipt) => _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(receipt.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicIssuedEvent>().Where(fact => fact.PaymentFrameId == receipt.Payment.PaymentFrameId).ToArray()is [var e] && e.OwnerFrameId == receipt.OwnerFrameId && e.CardActionId == receipt.CardActionId && e.ActorSeat == receipt.Payment.Source.OwnerSeat && e.MethodLedgerId == receipt.Payment.MethodLedgerId && e.ActualTurnNumber == receipt.Payment.ActualTurnNumber && e.ActualTurnOwnerSeat == receipt.Payment.ActualTurnOwnerSeat && e.NormalizedName == receipt.Payment.NormalizedName && e.EffectiveKind == receipt.Payment.EffectiveKind && e.Intent == receipt.Payment.Intent && e.Source == receipt.Payment.Source && e.GameplayHash == receipt.Payment.GameplayHash;
    internal bool IsIssuedDrawFundedDistinctBasicUse(CardUseFrame use)
    {
        if (use.DrawFundedDistinctBasicUse is not { } receipt || receipt.OwnerFrameId != use.Id || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !IsDrawFundedDistinctBasicIssued(receipt) || use.Action is not { Type: CardActionType.Use } current)
            return false;
        var action = use.CurrentSlashFirePolicy is { } fire ? fire.OriginalAction : current;
        if (use.CurrentSlashFirePolicy is not null)
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        var p = receipt.Payment;
        return action.ActionId == receipt.CardActionId && action.EffectiveKind == p.EffectiveKind && (use.CardKind == p.EffectiveKind || use.CurrentSlashFirePolicy is not null && CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind)) && action.ProviderSeat == p.Source.OwnerSeat && action.RequesterSeat is null && action.ResponderSeat is null && action.PhysicalCards.Count == 0 && action.ConversionChain is [var source] && source == p.Source && action.EffectiveSuit == Suit.None && action.EffectiveRank == 0 && action.EffectiveIsRed == false && (action.ActorSeat == p.Source.OwnerSeat && use.SourceSeat == p.Source.OwnerSeat || _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, p.Source.OwnerSeat));
    }

    internal bool IsDrawFundedDistinctBasicUse(long frameId) => _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { } use && IsIssuedDrawFundedDistinctBasicUse(use);
    internal Card DrawFundedDistinctBasicRepresentation(long frameId) => _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { } use && IsIssuedDrawFundedDistinctBasicUse(use) ? new(0, use.CardKind, Suit.None, 0) : throw new InvalidOperationException("A logical basic card lost its unique paid issuance.");
    internal bool ContinueDrawFundedDistinctBasicSimpleUse(long frameId, ProgramSimpleCardContinuation continuation)
    {
        if (continuation.CardId != 0 || !IsDrawFundedDistinctBasicUse(frameId))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = DrawFundedDistinctBasicRepresentation(frameId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(frameId, ResolutionFrameStep.ResolvingEffect);
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(frameId, card, use.CardKind);
            return true;
        }

        if (continuation.Effect == SimpleCardUseEffect.Recovery && use.CardKind is CardKind.Peach or CardKind.Alcohol)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, frameId, use.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (continuation.Effect == SimpleCardUseEffect.Alcohol && use.CardKind == CardKind.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, frameId);
        else
            throw new InvalidOperationException("A paid basic card changed its accepted native simple effect.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 150)]
    internal bool FinishDrawFundedDistinctBasicAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsDrawFundedDistinctBasicUse(attack.ResolutionId) || !CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }
}
