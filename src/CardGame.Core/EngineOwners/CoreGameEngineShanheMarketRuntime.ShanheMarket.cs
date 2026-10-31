#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheMarketRuntime
{
    internal void InitializeShanheBattleMarket()
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle?.MarketSetup is not { } setup)
            return;
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey(ShanheMarketPrograms.ExecutionSkillId))
            throw new InvalidOperationException("The live campaign market has no registered native acquisition program.");
        _shanheMarketOperations._shanheMarket = ShanheCampaignSession.CreateBattleMarketSession(setup, _ => _contentRegistryOperations._contentRegistry);
    }

    internal string? ShanheMarketInputError(int actor)
    {
        if (_shanheMarketOperations._shanheMarket is null || _coreMatchDefinitionOptionsAccess._options.ShanheBattle?.MarketSetup is null || actor != 0 || _coreMatchDefinitionOptionsAccess._options.HumanSeat != 0 || !_coreMatchParticipantsPlayersAccess._players[0].IsHuman)
            return "当前没有属于你的行军集市。";
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedAccess._started || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[0].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ShanheMarketPurchaseFrame>().Any())
            return "请等待当前购买结算完成，或开始新的征程。";
        return null;
    }

    internal CommandResult SubmitBuyShanheMarketItem(BuyShanheMarketItemCommand command)
    {
        var error = ShanheMarketInputError(command.ActorSeat) ?? ValidateLiveShanheMarketPurchase(command.ChoiceId, command.ReplaceSkillId);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, error);
        var item = _shanheMarketOperations._shanheMarket!.Snapshot.ShopItems.SingleOrDefault(i => i.ChoiceId == command.ChoiceId);
        if (item?.Reward is not { } reward)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "商品已经变化，请重新选择。");
        return _kernelCoreMainOperations.Accept(() =>
        {
            _shanheMarketOperations._shanheMarket.SynchronizeBattleMarketHealth(_coreMatchParticipantsPlayersAccess._players[0].Hp, _coreMatchParticipantsPlayersAccess._players[0].MaxHp);
            var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } pending ? CoreGameEngineCardAttackStateRuntime.CaptureSuspendedDecision(pending) : null;
            var status = _coreMatchOutcomeStatusWinnerAccess._status;
            var depth = _resolutionStackOperations._resolutionStack.Count;
            var parent = _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id;
            var discardQuote = _coreProgramHandLimitKindExemptionsOperations.Value.CaptureShanheMarketDiscardQuote();
            var bought = _shanheMarketOperations._shanheMarket.Submit(new Campaigns.BuyShanheMarketItemCommand(command.ChoiceId, command.ReplaceSkillId, _shanheMarketOperations._shanheMarket.Revision));
            if (!bought.Accepted)
                throw new InvalidOperationException(bought.Error ?? "The validated market purchase changed before payment.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            var frame = new ShanheMarketPurchaseFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, command.ChoiceId, reward.Id, command.ReplaceSkillId, item.GoldCost, item.HpCost, _shanheMarketOperations._shanheMarket.Revision, _shanheMarketOperations._shanheMarket.Snapshot.Gold, parent, depth, decision, status)
            {
                DiscardQuote = discardQuote
            };
            _coreRuntimeOperations.PushRuntimeFrame(frame);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMarketPurchaseCommittedEvent(frame.Id, frame.MarketRevision, frame.ChoiceId, frame.RewardId, frame.Price, frame.GoldAfter));
            ContinueShanheMarketPurchase(frame.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        });
    }

    internal string? ValidateLiveShanheMarketPurchase(string choiceId, string? replacement)
    {
        if (_shanheMarketOperations._shanheMarket is null)
            return "当前没有行军集市。";
        return _shanheMarketOperations._shanheMarket.ValidateMarketPurchase(choiceId, replacement, _coreMatchParticipantsPlayersAccess._players[0].Hp, _coreMatchParticipantsPlayersAccess._players[0].MaxHp);
    }

    internal CommandResult SubmitRefreshShanheMarket(RefreshShanheMarketCommand command)
    {
        var error = ShanheMarketInputError(command.ActorSeat) ?? _shanheMarketOperations._shanheMarket?.ValidateMarketRefresh();
        if (error is not null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, error);
        return _kernelCoreMainOperations.Accept(() =>
        {
            var result = _shanheMarketOperations._shanheMarket!.Submit(new RefreshShanheShopCommand(_shanheMarketOperations._shanheMarket.Revision));
            if (!result.Accepted)
                throw new InvalidOperationException(result.Error ?? "The validated market refresh changed before payment.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMarketRefreshedEvent(_shanheMarketOperations._shanheMarket.Revision, _shanheMarketOperations._shanheMarket.Snapshot.Gold));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        });
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 62030, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ShanheMarketPurchaseFrame), 62030)]
    internal void ContinueShanheMarketPurchase(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ShanheMarketPurchaseFrame market || market.Id != id)
            return;
        if (!ValidShanheMarketPurchase(market))
            throw new InvalidOperationException("A live market purchase lost its paid ledger owner.");
        if (!market.Applied)
        {
            if (market.ProgramFrameId is not null)
                throw new InvalidOperationException("A market purchase lost its active acquisition program.");
            var program = _contentRegistryOperations._contentRegistry.GetSkill(ShanheMarketPrograms.ExecutionSkillId).Program!;
            var trigger = program.Triggers.Single();
            _coreProgramLifecycleOperations.BeginProgramBinding(new(0, program.Id, trigger.Id, ShanheMarketPrograms.ExecutionGrantId, program.GameplayHash, trigger.Priority), new(SkillProgramTriggerWindow.SkillsChanged, market.Id, 0, SourceSeat: 0, TargetSeat: 0));
            return;
        }

        _coreRuntimeOperations.PopResolutionFrame(id, ResolutionFrameKind.ShanheMarketPurchase);
        if (_resolutionStackOperations._resolutionStack.Count != market.SuspendedStackDepth || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != market.SuspendedFrameId)
            throw new InvalidOperationException("A completed market purchase changed its suspended battle parent.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[0].IsAlive)
        {
            _coreMatchOutcomeStatusWinnerAccess._status = market.Status;
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = market.Decision is { } decision ? CoreGameEngineCardAttackStateRuntime.RestoreSuspendedDecision(decision) : null;
            RefreshShanheMarketSuspendedPrompt(market.DiscardQuote);
        }
    }

    internal bool ValidShanheMarketPurchase(ShanheMarketPurchaseFrame f) => _shanheMarketOperations._shanheMarket is not null && _coreMatchDefinitionOptionsAccess._options.ShanheBattle?.MarketSetup is not null && f.MarketRevision == _shanheMarketOperations._shanheMarket.Revision && f.GoldAfter == _shanheMarketOperations._shanheMarket.Snapshot.Gold && f.Price >= 0 && _shanheMarketOperations._shanheMarket.AcceptedCommands.LastOrDefault()is Campaigns.BuyShanheMarketItemCommand command && command.ChoiceId == f.ChoiceId && command.ReplaceSkillId == f.ReplaceSkillId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheMarketPurchaseCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid.MarketRevision == f.MarketRevision && paid.ChoiceId == f.ChoiceId && paid.RewardId == f.RewardId && paid.Price == f.Price && paid.GoldAfter == f.GoldAfter;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62030)]
    internal bool AllowsShanheMarketAcquisition(ProgramTriggerLegalityContext gate)
    {
        if (gate.Candidate.SkillId != ShanheMarketPrograms.ExecutionSkillId)
            return true;
        return gate.Candidate.BindingId == ShanheMarketPrograms.PurchaseTriggerId && gate.Candidate.OwnerSeat == 0 && gate.Candidate.SkillInstanceId == ShanheMarketPrograms.ExecutionGrantId && gate.Context.Window == SkillProgramTriggerWindow.SkillsChanged && _resolutionStackOperations._resolutionStack.LastOrDefault()is ShanheMarketPurchaseFrame { Applied: false } market && market.Id == gate.Context.ParentFrameId && ValidShanheMarketPurchase(market);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 62030)]
    internal bool IsIssuedShanheMarketCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == ShanheMarketPrograms.ExecutionSkillId && candidate.OwnerSeat == 0 && candidate.SkillInstanceId == ShanheMarketPrograms.ExecutionGrantId && candidate.BindingId == ShanheMarketPrograms.PurchaseTriggerId && candidate.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.GameplayHash && context.Window == SkillProgramTriggerWindow.SkillsChanged && context.OwnerSeat == 0 && _resolutionStackOperations._resolutionStack.LastOrDefault()is ShanheMarketPurchaseFrame { Applied: false, ProgramFrameId: null } market && market.Id == context.ParentFrameId && ValidShanheMarketPurchase(market);
    internal bool IsExactShanheMarketExecution(ProgramSkillFrame program)
    {
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId || program.OwnerSeat != 0 || program.TriggerId != ShanheMarketPrograms.PurchaseTriggerId || program.SkillInstanceId != ShanheMarketPrograms.ExecutionGrantId || program.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!.GameplayHash || program.WindowContext is not { Window: SkillProgramTriggerWindow.SkillsChanged } context)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == program.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ShanheMarketPurchaseFrame market && market.Id == context.ParentFrameId && (market.ProgramFrameId is null || market.ProgramFrameId == program.Id) && ValidShanheMarketPurchase(market);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ContinueDetachedProgramBinding, 62030)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62030)]
    internal bool KeepsPaidShanheMarketExecution(ProgramSkillFrame frame) => IsExactShanheMarketExecution(frame);
    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 62030)]
    internal bool ReturnShanheMarketAcquisition(ProgramSkillFrame program, bool completed)
    {
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ShanheMarketPurchaseFrame market || program.WindowContext?.ParentFrameId != market.Id || market.ProgramFrameId != program.Id || !ValidShanheMarketPurchase(market) || !completed || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheMarketPurchaseAppliedEvent>().Count(e => e.PurchaseFrameId == market.Id && e.RewardId == market.RewardId) != 1)
            throw new InvalidOperationException("A market acquisition did not return its exact applied purchase.");
        _coreRuntimeOperations.ReplaceRuntimeTop(market with { Applied = true });
        ContinueShanheMarketPurchase(market.Id);
        return true;
    }
    internal int ShanheMarketRuleBonus(int seat, SkillRuleQuery query) => seat != 0 || _shanheMarketOperations._shanheMarket is null || _coreMatchDefinitionOptionsAccess._options.ShanheBattle?.MarketSetup is not { } setup ? 0 : query switch
    {
        SkillRuleQuery.DrawCount => _shanheMarketOperations._shanheMarket.Snapshot.Bonuses.Draw - setup.State.Bonuses.Draw,
        SkillRuleQuery.SlashLimit => _shanheMarketOperations._shanheMarket.Snapshot.Bonuses.Slash - setup.State.Bonuses.Slash,
        _ => 0
    };
    internal void RefreshShanheMarketSuspendedPrompt(ShanheMarketDiscardQuote? discardQuote)
    {
        if (_coreTargetHandVisibilityOperations.Value.RefreshVisibleTargetHandSelectionPrompt())
            return;
        if (_coreShanheAlliedTricksOperations.RefreshShanheAlliedHarvestSelection())
            return;
        if (_coreProgramHandLimitKindExemptionsOperations.Value.RefreshShanheMarketDiscardPrompt(discardQuote))
            return;
        if (_coreRevealedFireAttackPaymentOperations.RefreshRevealedFireAttackPaymentPrompt())
            return;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } pending || pending.PlayerSeat != 0)
            return;
        var actor = _coreMatchParticipantsPlayersAccess._players[0];
        if (pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash)
        {
            var required = pending.RequiredCardKind ?? (pending.Kind == DecisionKind.RespondDodge ? CardKind.Dodge : CardKind.Slash);
            var cards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(actor, required);
            var bagua = pending.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "bagua");
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = Array.AsReadOnly(cards.Select(c => c.Id).ToArray()),
                Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(cards, required, pending.IncomingCard, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(actor, card, required), bagua, responder: actor)
            };
        }
        else if (pending.Kind == DecisionKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counter)
        {
            if (_coreShanheTrickResponseOperations.ShanheTrickNullificationBanned(actor.Seat))
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
                _coreNativeNullificationOperations.ContinueNullificationWindow(counter);
                return;
            }

            var cards = _coreNativeNullificationOperations.GetNullificationCards(actor);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = Array.AsReadOnly(cards.Select(c => c.Id).ToArray()),
                Choices = _coreNativeNullificationOperations.CreateNullificationChoices(counter, actor, cards)
            };
        }
        else if (pending.Kind == DecisionKind.RescueDying && _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0)
            {
                _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
                _coreNativeDyingOperations.CompleteDying(dying, survived: true);
            }
            else
                _coreNativeDyingOperations.RequestHumanDyingResponse(actor, _coreNativeDyingOperations.GetDyingPeaches(actor), _coreNativeDyingOperations.GetDyingAlcohols(actor, dying.VictimSeat));
        }
        else if (pending.Kind == DecisionKind.PlayCard && _coreActualTurnProgressCurrentSeatPhaseStartedAccess._currentSeat == 0 && _coreActualTurnProgressCurrentSeatPhaseStartedAccess._phase == TurnPhase.Play)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativePromptBoundaryOperations.RequestHumanPlay();
        }
        else
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
            };
    }
}
