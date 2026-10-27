using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ShanheCampaignSession? _shanheMarket;
    private static readonly CardMoveReason ShanheMarketCardGain = new("shanhe.market.purchase");
    public bool HasPendingShanheMarketPurchase
    {
        get { EnsureSessionHealthy(); return _resolutionStack.OfType<ShanheMarketPurchaseFrame>().Any(); }
    }

    private void InitializeShanheBattleMarket()
    {
        if (_options.ShanheBattle?.MarketSetup is not { } setup) return;
        if (!_contentRegistry.Skills.ContainsKey(ShanheMarketPrograms.ExecutionSkillId))
            throw new InvalidOperationException("The live campaign market has no registered native acquisition program.");
        _shanheMarket = ShanheCampaignSession.CreateBattleMarketSession(setup, _ => _contentRegistry);
    }

    /// <summary>The expedition trader belongs to its human owner; it contains no enemy private state.</summary>
    public ShanheCampaignSnapshot? CreateShanheMarketSnapshot(int viewerSeat)
    {
        EnsureSessionHealthy();
        return viewerSeat == 0 && _options.HumanSeat == 0
            ? _shanheMarket?.CreateBattleMarketSnapshot(_players[0].Hp, _players[0].MaxHp) : null;
    }

    /// <summary>Trusted-host result, imported by the campaign only after its battle journal was verified.</summary>
    public ShanheBattleMarketTransfer? CreateShanheMarketTransfer()
    {
        EnsureSessionHealthy();
        return _shanheMarket?.CreateBattleMarketTransfer();
    }

    private string? ShanheMarketInputError(int actor)
    {
        if (_shanheMarket is null || _options.ShanheBattle?.MarketSetup is null ||
            actor != 0 || _options.HumanSeat != 0 || !_players[0].IsHuman)
            return "当前没有属于你的行军集市。";
        if (!_started || !_setupComplete || _winner != Winner.None || !_players[0].IsAlive ||
            _resolutionStack.OfType<ShanheMarketPurchaseFrame>().Any())
            return "请等待当前购买结算完成，或开始新的征程。";
        return null;
    }

    private CommandResult SubmitBuyShanheMarketItem(BuyShanheMarketItemCommand command)
    {
        var error = ShanheMarketInputError(command.ActorSeat) ??
            ValidateLiveShanheMarketPurchase(command.ChoiceId, command.ReplaceSkillId);
        if (error is not null) return Reject(CommandErrorCode.IllegalAction, error);
        var item = _shanheMarket!.Snapshot.ShopItems.SingleOrDefault(i => i.ChoiceId == command.ChoiceId);
        if (item?.Reward is not { } reward) return Reject(CommandErrorCode.IllegalAction, "商品已经变化，请重新选择。");
        return Accept(() =>
        {
            _shanheMarket.SynchronizeBattleMarketHealth(_players[0].Hp, _players[0].MaxHp);
            var decision = _pendingDecision is { } pending ? CaptureSuspendedDecision(pending) : null;
            var status = _status; var depth = _resolutionStack.Count;
            var parent = _resolutionStack.LastOrDefault()?.Id;
            var discardQuote = CaptureShanheMarketDiscardQuote();
            var bought = _shanheMarket.Submit(new Campaigns.BuyShanheMarketItemCommand(
                command.ChoiceId, command.ReplaceSkillId, _shanheMarket.Revision));
            if (!bought.Accepted) throw new InvalidOperationException(bought.Error ?? "The validated market purchase changed before payment.");
            ClearPendingDecision();
            var frame = new ShanheMarketPurchaseFrame(++_resolutionSequence, command.ChoiceId, reward.Id,
                command.ReplaceSkillId, item.GoldCost, item.HpCost, _shanheMarket.Revision, _shanheMarket.Snapshot.Gold,
                parent, depth, decision, status) { DiscardQuote = discardQuote };
            PushRuntimeFrame(frame);
            AdvanceEventRulesAndQueueFact(new ShanheMarketPurchaseCommittedEvent(frame.Id, frame.MarketRevision,
                frame.ChoiceId, frame.RewardId, frame.Price, frame.GoldAfter));
            ContinueShanheMarketPurchase(frame.Id);
            AdvanceRulesAndPublishState();
        });
    }

    private string? ValidateLiveShanheMarketPurchase(string choiceId, string? replacement)
    {
        if (_shanheMarket is null) return "当前没有行军集市。";
        return _shanheMarket.ValidateMarketPurchase(choiceId, replacement, _players[0].Hp, _players[0].MaxHp);
    }

    private CommandResult SubmitRefreshShanheMarket(RefreshShanheMarketCommand command)
    {
        var error = ShanheMarketInputError(command.ActorSeat) ?? _shanheMarket?.ValidateMarketRefresh();
        if (error is not null) return Reject(CommandErrorCode.IllegalAction, error);
        return Accept(() =>
        {
            var result = _shanheMarket!.Submit(new RefreshShanheShopCommand(_shanheMarket.Revision));
            if (!result.Accepted) throw new InvalidOperationException(result.Error ?? "The validated market refresh changed before payment.");
            AdvanceEventRulesAndQueueFact(new ShanheMarketRefreshedEvent(_shanheMarket.Revision, _shanheMarket.Snapshot.Gold));
            AdvanceRulesAndPublishState();
        });
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 62030,
        NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ShanheMarketPurchaseFrame), 62030)]
    private void ContinueShanheMarketPurchase(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ShanheMarketPurchaseFrame market || market.Id != id) return;
        if (!ValidShanheMarketPurchase(market)) throw new InvalidOperationException("A live market purchase lost its paid ledger owner.");
        if (!market.Applied)
        {
            if (market.ProgramFrameId is not null) throw new InvalidOperationException("A market purchase lost its active acquisition program.");
            var program = _contentRegistry.GetSkill(ShanheMarketPrograms.ExecutionSkillId).Program!;
            var trigger = program.Triggers.Single();
            BeginProgramBinding(new(0, program.Id, trigger.Id, ShanheMarketPrograms.ExecutionGrantId,
                program.GameplayHash, trigger.Priority),
                new(SkillProgramTriggerWindow.SkillsChanged, market.Id, 0, SourceSeat: 0, TargetSeat: 0));
            return;
        }
        PopResolutionFrame(id, ResolutionFrameKind.ShanheMarketPurchase);
        if (_resolutionStack.Count != market.SuspendedStackDepth || _resolutionStack.LastOrDefault()?.Id != market.SuspendedFrameId)
            throw new InvalidOperationException("A completed market purchase changed its suspended battle parent.");
        if (_winner == Winner.None && _players[0].IsAlive)
        {
            _status = market.Status;
            _pendingDecision = market.Decision is { } decision ? RestoreSuspendedDecision(decision) : null;
            RefreshShanheMarketSuspendedPrompt(market.DiscardQuote);
        }
    }

    private bool ValidShanheMarketPurchase(ShanheMarketPurchaseFrame f) =>
        _shanheMarket is not null && _options.ShanheBattle?.MarketSetup is not null &&
        f.MarketRevision == _shanheMarket.Revision && f.GoldAfter == _shanheMarket.Snapshot.Gold &&
        f.Price >= 0 && _shanheMarket.AcceptedCommands.LastOrDefault() is Campaigns.BuyShanheMarketItemCommand command &&
        command.ChoiceId == f.ChoiceId && command.ReplaceSkillId == f.ReplaceSkillId &&
        ProgramEventHistory<ShanheMarketPurchaseCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray() is [var paid] &&
        paid.MarketRevision == f.MarketRevision && paid.ChoiceId == f.ChoiceId && paid.RewardId == f.RewardId &&
        paid.Price == f.Price && paid.GoldAfter == f.GoldAfter;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62030)]
    private bool AllowsShanheMarketAcquisition(ProgramTriggerLegalityContext gate)
    {
        if (gate.Candidate.SkillId != ShanheMarketPrograms.ExecutionSkillId) return true;
        return gate.Candidate.BindingId == ShanheMarketPrograms.PurchaseTriggerId && gate.Candidate.OwnerSeat == 0 &&
            gate.Candidate.SkillInstanceId == ShanheMarketPrograms.ExecutionGrantId &&
            gate.Context.Window == SkillProgramTriggerWindow.SkillsChanged &&
            _resolutionStack.LastOrDefault() is ShanheMarketPurchaseFrame { Applied: false } market &&
            market.Id == gate.Context.ParentFrameId && ValidShanheMarketPurchase(market);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 62030)]
    private bool IsIssuedShanheMarketCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        candidate.SkillId == ShanheMarketPrograms.ExecutionSkillId && candidate.OwnerSeat == 0 &&
        candidate.SkillInstanceId == ShanheMarketPrograms.ExecutionGrantId &&
        candidate.BindingId == ShanheMarketPrograms.PurchaseTriggerId &&
        candidate.GameplayHash == _contentRegistry.GetSkill(candidate.SkillId).Program!.GameplayHash &&
        context.Window == SkillProgramTriggerWindow.SkillsChanged && context.OwnerSeat == 0 &&
        _resolutionStack.LastOrDefault() is ShanheMarketPurchaseFrame { Applied: false, ProgramFrameId: null } market &&
        market.Id == context.ParentFrameId && ValidShanheMarketPurchase(market);

    private bool IsExactShanheMarketExecution(ProgramSkillFrame program)
    {
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId || program.OwnerSeat != 0 ||
            program.TriggerId != ShanheMarketPrograms.PurchaseTriggerId || program.SkillInstanceId != ShanheMarketPrograms.ExecutionGrantId ||
            program.GameplayHash != _contentRegistry.GetSkill(program.SkillId).Program!.GameplayHash ||
            program.WindowContext is not { Window: SkillProgramTriggerWindow.SkillsChanged } context) return false;
        var index = _resolutionStack.FindIndex(f => f.Id == program.Id);
        return index > 0 && _resolutionStack[index - 1] is ShanheMarketPurchaseFrame market &&
            market.Id == context.ParentFrameId && (market.ProgramFrameId is null || market.ProgramFrameId == program.Id) &&
            ValidShanheMarketPurchase(market);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ContinueDetachedProgramBinding, 62030)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62030)]
    private bool KeepsPaidShanheMarketExecution(ProgramSkillFrame frame) => IsExactShanheMarketExecution(frame);

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 62030)]
    private bool ReturnShanheMarketAcquisition(ProgramSkillFrame program, bool completed)
    {
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId) return false;
        if (_resolutionStack.LastOrDefault() is not ShanheMarketPurchaseFrame market ||
            program.WindowContext?.ParentFrameId != market.Id || market.ProgramFrameId != program.Id ||
            !ValidShanheMarketPurchase(market) || !completed ||
            ProgramEventHistory<ShanheMarketPurchaseAppliedEvent>().Count(e => e.PurchaseFrameId == market.Id && e.RewardId == market.RewardId) != 1)
            throw new InvalidOperationException("A market acquisition did not return its exact applied purchase.");
        ReplaceRuntimeTop(market with { Applied = true });
        ContinueShanheMarketPurchase(market.Id);
        return true;
    }

    private bool HasShanheMarketBoundary() => _resolutionStack.FirstOrDefault() is ShanheMarketPurchaseFrame market &&
        market.SuspendedStackDepth == 0 && market.SuspendedFrameId is null && ValidShanheMarketPurchase(market);

    private int ShanheMarketRuleBonus(int seat, SkillRuleQuery query) => seat != 0 || _shanheMarket is null ||
        _options.ShanheBattle?.MarketSetup is not { } setup ? 0 : query switch
        {
            SkillRuleQuery.DrawCount => _shanheMarket.Snapshot.Bonuses.Draw - setup.State.Bonuses.Draw,
            SkillRuleQuery.SlashLimit => _shanheMarket.Snapshot.Bonuses.Slash - setup.State.Bonuses.Slash,
            _ => 0
        };

    private void RefreshShanheMarketSuspendedPrompt(ShanheMarketDiscardQuote? discardQuote)
    {
        if (RefreshVisibleTargetHandSelectionPrompt()) return;
        if (RefreshShanheAlliedHarvestSelection()) return;
        if (RefreshShanheMarketDiscardPrompt(discardQuote)) return;
        if (RefreshRevealedFireAttackPaymentPrompt()) return;
        if (_pendingDecision is not { } pending || pending.PlayerSeat != 0) return;
        var actor = _players[0];
        if (pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash)
        {
            var required = pending.RequiredCardKind ?? (pending.Kind == DecisionKind.RespondDodge ? CardKind.Dodge : CardKind.Slash);
            var cards = GetResponseCards(actor, required);
            var bagua = pending.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "bagua");
            _pendingDecision = pending with { PromptId = CreatePromptId(), ValidCardIds = Array.AsReadOnly(cards.Select(c => c.Id).ToArray()),
                Choices = CreateResponseChoices(cards, required, pending.IncomingCard,
                    card => GetEffectiveResponseKind(actor, card, required), bagua, responder: actor) };
        }
        else if (pending.Kind == DecisionKind.Nullification && ActiveNullificationWindow is { } counter)
        {
            if (ShanheTrickNullificationBanned(actor.Seat))
            {
                ClearPendingDecision(); _status = EngineStatus.Running;
                ContinueNullificationWindow(counter); return;
            }
            var cards = GetNullificationCards(actor);
            _pendingDecision = pending with { PromptId = CreatePromptId(), ValidCardIds = Array.AsReadOnly(cards.Select(c => c.Id).ToArray()),
                Choices = CreateNullificationChoices(counter, actor, cards) };
        }
        else if (pending.Kind == DecisionKind.RescueDying && ActiveDying is { } dying)
        {
            ClearPendingDecision();
            if (_players[dying.VictimSeat].Hp > 0)
            {
                _status = EngineStatus.Running;
                CompleteDying(dying, survived: true);
            }
            else RequestHumanDyingResponse(actor, GetDyingPeaches(actor), GetDyingAlcohols(actor, dying.VictimSeat));
        }
        else if (pending.Kind == DecisionKind.PlayCard && _currentSeat == 0 && _phase == TurnPhase.Play)
        {
            ClearPendingDecision(); RequestHumanPlay();
        }
        else _pendingDecision = pending with { PromptId = CreatePromptId() };
    }
}

public sealed record ShanheMarketRefreshedEvent(long MarketRevision, int GoldAfter) : IGameEvent;
