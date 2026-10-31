#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal CardActionContext PrepareZonghuAction(CardActionContext a, ZonghuPaidOrigin? p) => p is null ? a : _coreProgramFactionActionRewardOperations.CaptureFactionAction(new(a.ActionId, a.ParentActionId, a.Type, p.Need.ActorSeat, p.Need.ProviderSeat, p.Need.RequesterSeat, null, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, [], [p.Source], a.DesignatedTargetSeats, Suit.None, 0, null));
    internal void IssueZonghuUse(CardUseFrame use, ZonghuPaidOrigin? p)
    {
        if (p is null)
            return;
        if (!ValidZonghuPayment(p) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) != 1 || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != p.OutputKind || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActorSeat != p.Need.ActorSeat || a.ProviderSeat != p.Need.ProviderSeat || a.RequesterSeat != p.Need.RequesterSeat || a.ConversionChain is not [var source] || source != p.Source || !use.TargetSeats.SequenceEqual([p.TargetSeat]) && !(p.OutputKind == CardKind.Alcohol && use.TargetSeats.Count == 0) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuResponseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId))
            throw new InvalidOperationException("Zonghu cannot issue a second zero-entity basic or change its real payer, actor or original native target.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZonghuOrigin = p });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuUseIssuedEvent(p.AttemptId, use.Id, a.ActionId, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, p.OutputKind));
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame { ZonghuReturn: { } r } parent && r.Origin == p && r.UseId == 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ZonghuReturn = r with { UseId = use.Id, ActionId = a.ActionId } });
    }

    internal bool IsZonghuUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } u && IsIssuedZonghuUse(u);
    internal bool IsIssuedZonghuUse(CardUseFrame use)
    {
        if (use.ZonghuOrigin is not { } p || !ValidZonghuPayment(p) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } current)
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            if (!CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind))
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }

        return (use.CardKind == p.OutputKind || use.CurrentSlashFirePolicy is not null && CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind)) && a.EffectiveKind == p.OutputKind && a.ConversionChain is [var source] && source == p.Source && a.ProviderSeat == p.Need.ProviderSeat && a.RequesterSeat == p.Need.RequesterSeat && a.ResponderSeat is null && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.EffectiveIsRed is null && ZonghuIssuedActorMatches(use, p) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseIssuedEvent>().Where(e => e.AttemptId == p.AttemptId).ToArray()is [var issued] && issued.UseId == use.Id && issued.ActionId == a.ActionId && issued.ActorSeat == p.Need.ActorSeat && issued.ProviderSeat == p.Need.ProviderSeat && issued.RequesterSeat == p.Need.RequesterSeat && issued.OutputKind == p.OutputKind;
    }

    internal bool ZonghuIssuedActorMatches(CardUseFrame use, ZonghuPaidOrigin p) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, p.Need.ActorSeat, p.Need.ProviderSeat) : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && use.SourceSeat == p.Need.ActorSeat && use.Action?.ActorSeat == p.Need.ActorSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Any(e => e.CardUseFrameId == use.Id);
    internal Card ZonghuRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } u && IsIssuedZonghuUse(u) ? new(0, u.CardKind, Suit.None, 0) : throw new InvalidOperationException("Zonghu representation requires its exact once-paid native zero use.");
    internal void DispatchZonghu(ZonghuPaidOrigin p)
    {
        if (!ValidZonghuPayment(p) || !_coreMatchParticipantsPlayersAccess._players[p.Need.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[p.Need.ProviderSeat].IsAlive)
            throw new InvalidOperationException("Zonghu cannot issue for a dead actor/provider.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var n = p.Need;
        if (n.Intent is ZonghuIntent.Dodge or ZonghuIntent.Duel or ZonghuIntent.Group or ZonghuIntent.FactionDodge or ZonghuIntent.FactionSlashPlayed)
        {
            DispatchZonghuResponse(p);
            return;
        }

        if (n.Intent == ZonghuIntent.Dying)
        {
            DispatchZonghuRecovery(p, true);
            return;
        }

        if (n.Intent == ZonghuIntent.Qinglong)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, n.ActorSeat, p.TargetSeat, true, [], p.OutputKind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
            BeginZonghuSlash(p, null, false, null);
            return;
        }

        if (n.Intent == ZonghuIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(b.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            b.AwaitingSlashChoice = false;
            b.SlashCardId = 0;
            b.EffectiveSlashKind = p.OutputKind;
            BeginZonghuSlash(p, b, true, null);
            return;
        }

        if (n.Intent == ZonghuIntent.Play)
        {
            BeginZonghuSlash(p, null, true, null);
            return;
        }

        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
        if (parent.InstructionIndex != n.InstructionIndex || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op != n.RequestOp)
            throw new InvalidOperationException("Zonghu requested use moved its original program cursor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { ZonghuReturn = new(p, 0, 0), Step = ResolutionFrameStep.ResolvingEffect });
        if (n.Intent == ZonghuIntent.GenericBasic)
        {
            var k = parent.KeBiNengPingduan!;
            var action = new LegalAction(CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) ? LegalActionKind.Slash : p.OutputKind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, 0, p.OutputKind == CardKind.Alcohol ? null : p.TargetSeat, "宗护：真实零实体基本牌", p.OutputKind, TargetSeats: p.OutputKind == CardKind.Alcohol ? [] : [p.TargetSeat])
            {
                ConversionSource = p.Source
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuKeActionAcceptedEvent(parent.Id, parent.InstructionIndex, k.Source, parent.GameplayHash, p.AttemptId, p.OutputKind, p.TargetSeat));
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { ZonghuKeAcceptedOrigin = p, KeBiNengPingduan = k with { BasicActions = k.BasicActions.Append(action).ToArray(), SelectedBasicAction = action, Stage = KeBiNengPingduanStage.BasicIssuing } });
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind))
                BeginZonghuSlash(p, null, true, parent.Id);
            else if (p.OutputKind == CardKind.Peach)
                DispatchZonghuRecovery(p, false);
            else
            {
                var id = _coreNativeCardUseOperations.BeginCardUse(new(0, p.OutputKind, Suit.None, 0), n.ActorSeat, [], p.OutputKind, physicalCardIds: [], conversionSource: p.Source, zonghuOrigin: p);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            }

            return;
        }

        PrepareZonghuProgram(parent, p);
        BeginZonghuSlash(p, null, true, parent.Id);
    }

    internal void PrepareZonghuProgram(ProgramSkillFrame parent, ZonghuPaidOrigin p)
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

        if (n.Intent == ZonghuIntent.ProgramSlash)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, n.ActorSeat, "宗护：视为使用杀。");
        else if (n.Intent == ZonghuIntent.AssistedSlash)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, "used-slash", n.ActorSeat, "宗护：视为使用杀。");
        }
        else if (n.Intent == ZonghuIntent.FixedSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { FixedTargetSlash = parent.FixedTargetSlash!with { Issued = true } });
        else if (n.Intent == ZonghuIntent.NearestLegalSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NearestLegalSlashRequest = null });
    }

    internal void BeginZonghuSlash(ZonghuPaidOrigin p, NativeOwners.BorrowedSwordHandle? borrowed, bool counts, long? parent, NativeOwners.FactionCardRequestHandle? faction = null)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Need.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.TargetSeat];
        var kind = p.OutputKind;
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, kind, Suit.None, 0), actor.Seat, [target.Seat], kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), physicalCardIds: [], conversionSource: p.Source, zonghuOrigin: p);
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

    internal void DispatchZonghuRecovery(ZonghuPaidOrigin p, bool dying)
    {
        var n = p.Need;
        var source = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.TargetSeat];
        var card = new Card(0, p.OutputKind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [target.Seat], p.OutputKind, physicalCardIds: [], conversionSource: p.Source, zonghuOrigin: p);
        if (dying)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { DyingResponse = new(n.OwnerFrameId, source.Seat, p.OutputKind == CardKind.Peach, p.OutputKind == CardKind.Peach ? 0 : null, p.OutputKind == CardKind.Alcohol, p.OutputKind == CardKind.Alcohol ? 0 : null) });
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Recovery));
    }

    internal bool ContinueZonghuSimpleUse(long id, ProgramSimpleCardContinuation c)
    {
        if (c.CardId != 0 || !IsZonghuUse(id))
            return false;
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[u.SourceSeat];
        var card = ZonghuRepresentation(id);
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
            throw new InvalidOperationException("Zonghu changed its native simple-card continuation.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 90)]
    internal bool FinishZonghuAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsZonghuUse(attack.ResolutionId) || !CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
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

    [NativeUseValidation(27512)]
    internal void ValidateZonghuNativeReturn(CardUseFrame use)
    {
        if (use.ZonghuOrigin is not null && (!IsIssuedZonghuUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Zonghu cannot retire before its real native use and children finish.");
    }

    [NativeUseReturn(18)]
    internal void ReturnZonghuNativeUse(CardUseFrame use)
    {
        if (use.ZonghuOrigin is not { } p)
            return;
        if (!IsIssuedZonghuUse(use) || _resolutionStackOperations._resolutionStack.Any(f => f.Id == use.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseReturnedEvent>().Any(e => e.AttemptId == p.AttemptId))
            throw new InvalidOperationException("Zonghu native return is not its exact once completed use.");
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.Need.OwnerFrameId)is ProgramSkillFrame { ZonghuReturn: { } r } parent && r.Origin == p && r.UseId == use.Id)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ZonghuReturn = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuUseReturnedEvent(p.AttemptId, use.Id, use.Action!.ActionId));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27513)]
    internal bool ResumeZonghuRequestedUse(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZonghuReturn: { Returned: true } r } parent || parent.Id != id)
            return false;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseReturnedEvent>().Count(e => e.AttemptId == r.Origin.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) != 1)
            throw new InvalidOperationException("Zonghu caller lost its once-completed typed return.");
        if (parent.SunYuan is not null || parent.Chanshuang is not null || parent.WuJing is not null || parent.LvKuangLvXiang is not null || parent.CaoChun is not null || parent.KeBiNengPingduan is not null)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { ZonghuReturn = null });
        if (parent.FixedTargetSlash is { Issued: true })
            _coreFixedSlashAndDeclaredDeckProgramsOperations.TryResumeFixedSlashAndDeclaredDeck(id);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27513)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27513)]
    internal bool ContinueZonghuOriginalProgram(ProgramSkillFrame f) => f.ZonghuReturn is { } r && ValidZonghuPayment(r.Origin) && r.Origin.Need.OwnerFrameId == f.Id && r.Origin.Need.InstructionIndex == f.InstructionIndex;
}
