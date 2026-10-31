#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal CardActionContext PrepareWeijieAction(CardActionContext a, WeijiePaidOrigin? p) => p is null ? a : _coreProgramFactionActionRewardOperations.CaptureFactionAction(new(a.ActionId, a.ParentActionId, a.Type, p.Need.ActorSeat, p.Need.ProviderSeat, p.Need.RequesterSeat, null, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, [], [p.Source], a.DesignatedTargetSeats, Suit.None, 0, null));
    internal void IssueWeijieUse(CardUseFrame use, WeijiePaidOrigin? p)
    {
        if (p is null)
            return;
        if (!ValidWeijiePayment(p) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) != 1 || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != p.OutputKind || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActorSeat != p.Need.ActorSeat || a.ProviderSeat != p.Need.ProviderSeat || a.RequesterSeat != p.Need.RequesterSeat || a.ConversionChain is not [var source] || source != p.Source || !use.TargetSeats.SequenceEqual([p.TargetSeat]) && !(p.OutputKind == CardKind.Alcohol && use.TargetSeats.Count == 0) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieUseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieResponseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId))
            throw new InvalidOperationException("Weijie cannot issue a second zero-entity basic or change its real payer, actor or original native target.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { WeijieOrigin = p });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieUseIssuedEvent(p.AttemptId, use.Id, a.ActionId, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, p.OutputKind));
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame { WeijieReturn: { } r } parent && r.Origin == p && r.UseId == 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WeijieReturn = r with { UseId = use.Id, ActionId = a.ActionId } });
    }

    internal bool IsWeijieUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } u && IsIssuedWeijieUse(u);
    internal bool IsIssuedWeijieUse(CardUseFrame use)
    {
        if (use.WeijieOrigin is not { } p || !ValidWeijiePayment(p) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } current)
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            if (!CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind))
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }

        return (use.CardKind == p.OutputKind || use.CurrentSlashFirePolicy is not null && CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind)) && a.EffectiveKind == p.OutputKind && a.ConversionChain is [var source] && source == p.Source && a.ProviderSeat == p.Need.ProviderSeat && a.RequesterSeat == p.Need.RequesterSeat && a.ResponderSeat is null && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.EffectiveIsRed is null && WeijieIssuedActorMatches(use, p) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieUseIssuedEvent>().Where(e => e.AttemptId == p.AttemptId).ToArray()is [var issued] && issued.UseId == use.Id && issued.ActionId == a.ActionId && issued.ActorSeat == p.Need.ActorSeat && issued.ProviderSeat == p.Need.ProviderSeat && issued.RequesterSeat == p.Need.RequesterSeat && issued.OutputKind == p.OutputKind;
    }

    internal bool WeijieIssuedActorMatches(CardUseFrame use, WeijiePaidOrigin p) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, p.Need.ActorSeat, p.Need.ProviderSeat) : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && use.SourceSeat == p.Need.ActorSeat && use.Action?.ActorSeat == p.Need.ActorSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Any(e => e.CardUseFrameId == use.Id);
    internal Card WeijieRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } u && IsIssuedWeijieUse(u) ? new(0, u.CardKind, Suit.None, 0) : throw new InvalidOperationException("Weijie representation requires its exact once-paid native zero use.");
    internal void DispatchWeijie(WeijiePaidOrigin p)
    {
        if (!ValidWeijiePayment(p) || !_coreMatchParticipantsPlayersAccess._players[p.Need.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[p.Need.ProviderSeat].IsAlive)
            throw new InvalidOperationException("Weijie cannot issue for a dead actor/provider.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var n = p.Need;
        if (n.Intent == WeijieIntent.WenyiPeach)
        {
            _coreWenyiPeachSuppliesOperations.ExecuteWenyiWeijiePeach(p);
            return;
        }

        if (n.Intent is WeijieIntent.Dodge or WeijieIntent.Duel or WeijieIntent.Group or WeijieIntent.FactionDodge or WeijieIntent.FactionSlashPlayed)
        {
            DispatchWeijieResponse(p);
            return;
        }

        if (n.Intent == WeijieIntent.Dying)
        {
            DispatchWeijieRecovery(p, true);
            return;
        }

        if (n.Intent == WeijieIntent.Qinglong)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, n.ActorSeat, p.TargetSeat, true, [], p.OutputKind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
            BeginWeijieSlash(p, null, false, null);
            return;
        }

        if (n.Intent == WeijieIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(b.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            b.AwaitingSlashChoice = false;
            b.SlashCardId = 0;
            b.EffectiveSlashKind = p.OutputKind;
            BeginWeijieSlash(p, b, true, null);
            return;
        }

        if (n.Intent == WeijieIntent.FactionSlashUse)
        {
            DispatchWeijieFactionUse(p);
            return;
        }

        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
        if (parent.InstructionIndex != n.InstructionIndex || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op != n.RequestOp)
            throw new InvalidOperationException("Weijie requested use moved its original program cursor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { WeijieReturn = new(p, 0, 0), Step = ResolutionFrameStep.ResolvingEffect });
        if (n.Intent == WeijieIntent.GenericBasic)
        {
            var k = parent.KeBiNengPingduan!;
            var action = new LegalAction(CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) ? LegalActionKind.Slash : p.OutputKind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, 0, p.OutputKind == CardKind.Alcohol ? null : p.TargetSeat, "诿解：真实零实体基本牌", p.OutputKind, TargetSeats: p.OutputKind == CardKind.Alcohol ? [] : [p.TargetSeat])
            {
                ConversionSource = p.Source
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieKeActionAcceptedEvent(parent.Id, parent.InstructionIndex, k.Source, parent.GameplayHash, p.AttemptId, p.OutputKind, p.TargetSeat));
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { WeijieKeAcceptedOrigin = p, KeBiNengPingduan = k with { BasicActions = k.BasicActions.Append(action).ToArray(), SelectedBasicAction = action, Stage = KeBiNengPingduanStage.BasicIssuing } });
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind))
                BeginWeijieSlash(p, null, false, parent.Id);
            else if (p.OutputKind == CardKind.Peach)
                DispatchWeijieRecovery(p, false);
            else
            {
                var id = _coreNativeCardUseOperations.BeginCardUse(new(0, p.OutputKind, Suit.None, 0), n.ActorSeat, [], p.OutputKind, physicalCardIds: [], conversionSource: p.Source, weijieOrigin: p);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            }

            return;
        }

        PrepareWeijieProgram(parent, p);
        BeginWeijieSlash(p, null, false, parent.Id);
    }

    internal void PrepareWeijieProgram(ProgramSkillFrame parent, WeijiePaidOrigin p)
    {
        var e = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)!;
        var n = p.Need;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(parent))
        {
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(parent, n.ActorSeat, n.ProviderSeat, p.TargetSeat, p.OutputKind, [], p.Source);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parent))
        {
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(parent, n.ActorSeat, n.ProviderSeat, p.TargetSeat, p.OutputKind, [], p.Source);
            return;
        }

        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { LvKuangLvXiang = parent.LvKuangLvXiang!with { Stage = "issuing", IssuingIds = [], IssuingKind = p.OutputKind, IssuingProvider = p.Need.ProviderSeat } });
            return;
        }

        if (_characterWuJingOlOperations.IsWuJingHejiRequest(parent))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WuJing = parent.WuJing!with { Stage = WuJingStage.IssuingUse, Selection = new(p.OutputKind, false, false) { Conversions = [p.Source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(parent))
        {
            var r = parent.CaoChun!;
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = new(parent.Id, r.Source, r.GameplayHash, r.InstructionIndex, r.ActualTurn, r.PhaseInstance, false, p.TargetSeat, p.OutputKind) { ProviderSeat = n.ProviderSeat, Materials = [], PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } } });
            return;
        }

        if (n.Intent == WeijieIntent.ProgramSlash)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, n.ActorSeat, "诿解：视为使用杀。");
        else if (n.Intent == WeijieIntent.AssistedSlash)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, "used-slash", n.ActorSeat, "诿解：视为使用杀。");
        }
        else if (n.Intent == WeijieIntent.FixedSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { FixedTargetSlash = parent.FixedTargetSlash!with { Issued = true } });
        else if (n.Intent == WeijieIntent.NearestLegalSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NearestLegalSlashRequest = null });
    }

    internal void BeginWeijieSlash(WeijiePaidOrigin p, NativeOwners.BorrowedSwordHandle? borrowed, bool counts, long? parent, NativeOwners.FactionCardRequestHandle? faction = null)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Need.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.TargetSeat];
        var kind = p.OutputKind;
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, kind, Suit.None, 0), actor.Seat, [target.Seat], kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), physicalCardIds: [], conversionSource: p.Source, weijieOrigin: p);
        var actual = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, actual.TargetSeats[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[actual.TargetSeats[0]], kind), programSkillCardUseFrameId: parent);
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        if (faction is not null)
            faction.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        if (counts && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && actual.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, p.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        _characterLvKuangLvXiangOlOperations.CompleteQigongNativePayment(id);
        _characterCaoChunOlOperations.CompleteCaoChunNativePayment(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, actual.TargetSeats[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[actual.TargetSeats[0]]);
        if (_coreCardSupplyCompletionOperations.TryBeginProvidedCardSupplyCompletion(attack, faction))
            return;
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void DispatchWeijieRecovery(WeijiePaidOrigin p, bool dying)
    {
        var n = p.Need;
        var source = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.TargetSeat];
        var card = new Card(0, p.OutputKind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [target.Seat], p.OutputKind, physicalCardIds: [], conversionSource: p.Source, weijieOrigin: p);
        if (dying)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { DyingResponse = new(n.OwnerFrameId, source.Seat, p.OutputKind == CardKind.Peach, p.OutputKind == CardKind.Peach ? 0 : null, p.OutputKind == CardKind.Alcohol, p.OutputKind == CardKind.Alcohol ? 0 : null) });
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Recovery));
    }

    internal bool ContinueWeijieSimpleUse(long id, ProgramSimpleCardContinuation c)
    {
        if (c.CardId != 0 || !IsWeijieUse(id))
            return false;
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[u.SourceSeat];
        var card = WeijieRepresentation(id);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, u.CardKind);
            return true;
        }

        if (c.Effect == SimpleCardUseEffect.Recovery && u.CardKind is CardKind.Peach or CardKind.Alcohol)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[u.TargetSeats[u.TargetIndex]], card, id, u.CardKind, c.RecoveryAmount, c.RecoveryPolicySources ?? []);
        else if (c.Effect == SimpleCardUseEffect.Alcohol && u.CardKind == CardKind.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        else
            throw new InvalidOperationException("Weijie changed its native simple-card continuation.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 50)]
    internal bool FinishWeijieAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsWeijieUse(attack.ResolutionId) || !CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
            return false;
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(u.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(u.Id, 0, u.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, u.Action!, SkillProgramTriggerWindow.CardUseCompleted, u.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(u.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(27462)]
    internal void ValidateWeijieNativeReturn(CardUseFrame use)
    {
        if (use.WeijieOrigin is not null && (!IsIssuedWeijieUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Weijie cannot retire before its real native use and children finish.");
    }

    [NativeUseReturn(19)]
    internal void ReturnWeijieNativeUse(CardUseFrame use)
    {
        if (use.WeijieOrigin is not { } p)
            return;
        if (!IsIssuedWeijieUse(use) || _resolutionStackOperations._resolutionStack.Any(f => f.Id == use.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieUseReturnedEvent>().Any(e => e.AttemptId == p.AttemptId))
            throw new InvalidOperationException("Weijie native return is not its exact once completed use.");
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.Need.OwnerFrameId)is ProgramSkillFrame { WeijieReturn: { } r } parent && r.Origin == p && r.UseId == use.Id)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WeijieReturn = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieUseReturnedEvent(p.AttemptId, use.Id, use.Action!.ActionId));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27463)]
    internal bool ResumeWeijieRequestedUse(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WeijieReturn: { Returned: true } r } parent || parent.Id != id)
            return false;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieUseReturnedEvent>().Count(e => e.AttemptId == r.Origin.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) != 1)
            throw new InvalidOperationException("Weijie caller lost its once-completed typed return.");
        if (_characterSunYuanOlOperations.TryGetSayingSlashRequest(parent, out _, out _) || parent.Chanshuang is not null || parent.WuJing is not null || parent.LvKuangLvXiang is not null || parent.CaoChun is not null || parent.KeBiNengPingduan is not null)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { WeijieReturn = null });
        if (parent.FixedTargetSlash is { Issued: true })
            _coreFixedSlashAndDeclaredDeckProgramsOperations.TryResumeFixedSlashAndDeclaredDeck(id);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27463)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27463)]
    internal bool ContinueWeijieOriginalProgram(ProgramSkillFrame f) => f.WeijieReturn is { } r && ValidWeijiePayment(r.Origin) && r.Origin.Need.OwnerFrameId == f.Id && r.Origin.Need.InstructionIndex == f.InstructionIndex;
}
