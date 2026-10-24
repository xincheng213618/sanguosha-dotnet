using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class GuanduBattleRulesFeature : IGuanduBattleRulesFeature, IMatchModeRuleObserverProvider,
    IMatchModeRuntimeHookProvider, IMatchModeNumericRules, IMatchModeLifecycle, IMatchModeSnapshotProjector,
    IMatchModeCardActionObserver, IMatchModeDamageRuleProvider, IMatchModeDamageNaturePolicy
{
    private readonly ContentModeDefinition _mode;
    private readonly IModeBattleRulesHost _host;
    private readonly IGuanduEventFlowHost _flow;
    private bool Active => _mode.GuanduActivity is not null;

    internal GuanduBattleRulesFeature(ContentModeDefinition mode, IModeBattleRulesHost host, IGuanduEventFlowHost flow)
    {
        (_mode, _host, _flow) = (mode, host, flow);
        RuleObservers = Active ? Array.AsReadOnly<ModeRuleObserverRegistration>(
        [
            ModeRuleObserverRegistration.ForEvent<CardMovedEvent>(ProgramRulePhase.AfterEvent, 46000, ObserveDeckExhaustion),
            ModeRuleObserverRegistration.ForEvent<IGameEvent>(ProgramRulePhase.AfterEvent, 46001, ObserveEventPhases),
            ModeRuleObserverRegistration.ForEvent<CardUseDeclaredEvent>(ProgramRulePhase.AfterEvent, 46002, ObserveDeclaredUse)
        ]) : [];
        RuntimeHooks = Active ? Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 46000, TryComplete)]) : [];
        DamageRules = Active ? Array.AsReadOnly<ModeDamageRuleRegistration>(
            [ModeDamageRuleRegistration.Additive(210, damage => DamageBonus(new(damage.ResolutionId, damage.SourceSeat,
                damage.TargetSeat, damage.CardUserSeat, damage.IsSourceLess, damage.IsChainPropagation, damage.EffectiveCardKind)))]) : [];
    }
    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
    public IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; }
    int IMatchModeNumericRules.Order => 10;
    int IMatchModeLifecycle.Order => 20;
    int IMatchModeSnapshotProjector.Order => 20;
    int IMatchModeCardActionObserver.Order => 20;
    int IMatchModeDamageNaturePolicy.Order => 210;
    public void OnCardAction(ModeCardActionFacts action, SkillProgramTriggerWindow window) =>
        RecordCardAction(new(action.ActionId, action.Type, action.ActorSeat, action.EffectiveKind), window);
    public GuanduBattleConfiguredEvent? Configuration => Active ?
        _host.History<GuanduBattleConfiguredEvent>().SingleOrDefault() ?? _host.InitialGuanduConfiguration : null;
    public GuanduBattleEvent? CurrentEvent => Configuration?.Event;
    public bool IsCommander(int seat) => Configuration is { } configuration &&
        (configuration.CaoCommanderSeat == seat || configuration.YuanCommanderSeat == seat);
    public bool Allies(int first, int second)
    {
        var players = _host.CaptureFacts().Players;
        return first >= 0 && first < players.Count && second >= 0 && second < players.Count &&
            players[first].TeamId is { } team && team == players[second].TeamId;
    }
    public GuanduActivitySnapshot? Snapshot => Configuration is { } configuration ? new(configuration.Event,
        _host.CaptureFacts().RoundNumber, configuration.PlayerTeamId == "team:cao" ? configuration.CaoCommanderSeat : configuration.YuanCommanderSeat,
        configuration.PlayerTeamId == "team:cao" ? configuration.YuanCommanderSeat : configuration.CaoCommanderSeat,
        _host.History<GuanduDeckExhaustedEvent>().Any(), configuration.PlayerTeamId, configuration.HumanRole, configuration.LayoutIndex) : null;
    public GameSnapshot ProjectGame(GameSnapshot snapshot, int viewerSeat, bool revealAll) => snapshot with { GuanduActivity = Snapshot };

    public void OnMatchStart(ModeMatchStartPhase phase)
    {
        if (Active && phase == ModeMatchStartPhase.AfterRunning && _host.InitialGuanduConfiguration is { } configuration)
            _host.PublishFact(configuration);
    }
    public bool TryHandleEmptyDrawPile()
    {
        if (!Active) return false;
        RecordDeckExhaustion();
        return true;
    }
    public void RecordDeckExhaustion()
    {
        if (!Active || _host.History<GuanduDeckExhaustedEvent>().Any()) return;
        _host.PublishFact(new GuanduDeckExhaustedEvent(_host.CaptureFacts().TurnNumber));
    }
    private void ObserveDeckExhaustion(CardMovedEvent fact)
    {
        if (_host.CaptureFacts() is { SetupComplete: true, DrawPileCount: 0 }) RecordDeckExhaustion();
    }
    public void ApplyDeathReward(int victimSeat, int? killerSeat)
    {
        if (!Active || killerSeat is not { } killer || killer == victimSeat || !_host.CaptureFacts().Players[killer].IsAlive) return;
        if (!Allies(victimSeat, killer)) _host.TransferAllHandCards(victimSeat, killer, "guandu.enemy-kill-hand");
        else _host.DiscardAllHandAndEquipment(killer, "guandu.friendly-kill");
    }
    public bool TryCompleteMatch(ModeCompletionBoundary boundary) => boundary == ModeCompletionBoundary.AfterTurnEnded && TryComplete();
    private bool TryComplete()
    {
        if (Configuration is not { } configuration) return false;
        var facts = _host.CaptureFacts();
        if (!facts.SetupComplete || !facts.StartingProgramsResolved || facts.HasWinner || facts.ResolutionCount != 0 ||
            facts.PendingDecisionKind is { } decision && decision != DecisionKind.PlayCard) return false;
        var exhausted = _host.History<GuanduDeckExhaustedEvent>().Any();
        var caoAlive = facts.Players[configuration.CaoCommanderSeat].IsAlive;
        var yuanAlive = facts.Players[configuration.YuanCommanderSeat].IsAlive;
        if (caoAlive && yuanAlive && !exhausted) return false;
        if (facts.PendingMovementBatches != 0 || facts.PendingHpChanges != 0)
        { _host.ClearDecision(); _host.PublishState(); return true; }
        var team = exhausted ? configuration.PlayerTeamId == "team:cao" ? "team:yuan" : "team:cao" :
            !caoAlive && !yuanAlive ? null : caoAlive ? "team:cao" : "team:yuan";
        var winner = team is null ? Winner.Draw : _host.WinnerForTeam(team);
        _host.SetWinner(winner, team);
        _host.PublishFact(new WinnerDeterminedEvent(winner, team));
        _host.CompleteGame();
        return true;
    }

    public void RecordCardAction(ModeBattleCardAction action, SkillProgramTriggerWindow window)
    {
        if (!Active || window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted) ||
            _host.History<GuanduActualCardActionEvent>().Any(fact => fact.ActionId == action.ActionId)) return;
        var use = _host.FindUseByAction(action.ActionId);
        var logicalUse = window == SkillProgramTriggerWindow.CardUseCommitted || action.Type == CardActionType.Use;
        var ordinal = _host.History<GuanduActualCardActionEvent>().Count(fact => fact.CountsAsUse) + (logicalUse ? 1 : 0);
        var facts = _host.CaptureFacts();
        _host.PublishFact(new GuanduActualCardActionEvent(action.ActionId, use?.FrameId ?? 0, action.Type,
            action.ActorSeat, action.Kind, facts.TurnNumber, facts.ActualPhase, ordinal) { CountsAsUse = logicalUse });
    }
    private void ObserveDeclaredUse(CardUseDeclaredEvent declared)
    {
        if (_host.FindUse(declared.ResolutionId) is { Action.Type: CardActionType.Use } use)
            RecordCardAction(use.Action, SkillProgramTriggerWindow.CardUseCommitted);
    }
    public DamageNature TransformDamageNature(DamageNature nature) => CurrentEvent == GuanduBattleEvent.WuchaoFire &&
        nature == DamageNature.Normal ? DamageNature.Fire : nature;
    public int DamageBonus(ModeActivityDamageFacts damage)
    {
        if (damage.IsSourceLess || !Active) return 0;
        var facts = _host.CaptureFacts();
        if (CurrentEvent == GuanduBattleEvent.Underdog)
            return facts.Players[damage.TargetSeat].Hp > facts.Players[damage.SourceSeat].Hp ? 1 : 0;
        if (CurrentEvent != GuanduBattleEvent.Standoff || facts.RoundNumber <= 4 || damage.IsChainPropagation ||
            damage.SourceSeat != facts.CurrentSeat || damage.CardUserSeat != damage.SourceSeat ||
            damage.EffectiveCardKind is not { } kind || !_host.IsSlash(kind)) return 0;
        return _host.History<GuanduActualCardActionEvent>().FirstOrDefault(fact => fact.ActualTurnNumber == facts.TurnNumber &&
            fact.ActorSeat == damage.SourceSeat && fact.Type == CardActionType.Use && _host.IsSlash(fact.Kind))?.UseFrameId == damage.ResolutionId ? 1 : 0;
    }
    public int IntoxicationDamageBonus(int targetSeat)
    {
        if (!Active) return 0;
        var facts = _host.CaptureFacts();
        return facts.Phase is TurnPhase.NotStarted or TurnPhase.Finished ? 0 : _host.History<GuanduIntoxicationAppliedEvent>()
            .Count(fact => fact.ActualTurnNumber == facts.TurnNumber && fact.TargetSeat == targetSeat);
    }

    private int SlowEligibleCredits(int seat) => _host.History<GuanduSlowAdvanceEarnedEvent>().Where(fact => fact.Phase.ActualTurnOwnerSeat == seat)
        .Skip(_host.History<GuanduSlowAdvanceConsumedEvent>().Where(fact => fact.OwnerSeat == seat).Sum(fact => fact.Credits))
        .Count(fact => fact.Phase.TurnNumber < _host.CaptureFacts().TurnNumber);
    private int HoldCredits(int seat, long? excludedToken = null) => _host.History<GuanduHoldLineUsedEvent>().Where(fact => fact.OwnerSeat == seat)
        .Skip(_host.History<GuanduHoldLineConsumedEvent>().Where(fact => fact.Phase.ActualTurnOwnerSeat == seat).Sum(fact => fact.Credits))
        .Count(fact => excludedToken is null || fact.ExcludedDiscardPhaseToken != excludedToken);
    private long? CurrentDiscardToken => _host.CaptureFacts().ActualPhase is { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase &&
        !_host.History<ActualPhaseEndedEvent>().Any(fact => fact.Phase == phase) ? phase.Token : null;
    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        if (_mode.GuanduActivity is not { } activity) yield break;
        var tier = query switch { SkillRuleQuery.InitialHandSize => activity.PlayerTier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => activity.PlayerTier >= 4 ? 1 : 0, SkillRuleQuery.SlashLimit => activity.PlayerTier >= 3 ? 1 : 0, _ => 0 };
        if (tier != 0) yield return new FiniteRuleQueryContribution($"guandu:tier:{query}", SkillRuleOperation.Add, tier);
        var facts = _host.CaptureFacts();
        var amount = query switch
        {
            SkillRuleQuery.DrawCount when CurrentEvent == GuanduBattleEvent.GrainShortage => -1,
            SkillRuleQuery.DrawCount when CurrentEvent == GuanduBattleEvent.SlowAdvance => SlowEligibleCredits(seat) > 0 && !facts.IsExtraDrawPhase ? 1 : 0,
            SkillRuleQuery.HandLimit when CurrentEvent == GuanduBattleEvent.Standoff && facts.RoundNumber <= 4 => facts.RoundNumber,
            SkillRuleQuery.HandLimit when CurrentEvent == GuanduBattleEvent.HoldTheLine => -HoldCredits(seat, CurrentDiscardToken),
            _ => 0
        };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"guandu:event:{query}", SkillRuleOperation.Add, amount);
        foreach (var contribution in _host.NativeGeneralContributions(seat, query)) yield return contribution;
    }
    private void ObserveEventPhases(IGameEvent fact)
    {
        if (fact is ActualPhaseEndedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Play } ended && CurrentEvent == GuanduBattleEvent.SlowAdvance &&
            !_host.History<GuanduActualCardActionEvent>().Any(action => action.Phase == ended.Phase &&
                action.ActorSeat == ended.Phase.ActualTurnOwnerSeat && _host.IsSlash(action.Kind))) _host.PublishFact(new GuanduSlowAdvanceEarnedEvent(ended.Phase));
        if (fact is ActualDrawPhaseCompletedEvent { IsExtra: false, Skipped: false } draw && SlowEligibleCredits(draw.OwnerSeat) > 0)
            _host.PublishFact(new GuanduSlowAdvanceConsumedEvent(draw.FrameId, draw.OwnerSeat, SlowEligibleCredits(draw.OwnerSeat)));
        if (fact is ActualPhaseEndedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Discard } discarded &&
            HoldCredits(discarded.Phase.ActualTurnOwnerSeat, discarded.Phase.Token) > 0) _host.PublishFact(new GuanduHoldLineConsumedEvent(
                discarded.Phase, HoldCredits(discarded.Phase.ActualTurnOwnerSeat, discarded.Phase.Token)));
    }
    public bool CanUseRoundConversion(string skillId, string bindingId, int ownerSeat) => skillId != "guandu:battle-event" || bindingId != "hold-line" ||
        CurrentEvent == GuanduBattleEvent.HoldTheLine && !_host.History<GuanduHoldLineUsedEvent>().Any(fact => fact.OwnerSeat == ownerSeat && fact.RoundNumber == _host.CaptureFacts().RoundNumber);
    public void ConsumeRoundConversion(CardConversionSource source)
    {
        if (source.SkillId != "guandu:battle-event" || source.BindingId != "hold-line") return;
        var facts = _host.CaptureFacts();
        if (CurrentEvent != GuanduBattleEvent.HoldTheLine || _host.History<GuanduHoldLineUsedEvent>().Any(fact => fact.OwnerSeat == source.OwnerSeat && fact.RoundNumber == facts.RoundNumber))
            throw new InvalidOperationException("The actual round's Slash-as-Dodge allowance was already paid.");
        _host.PublishFact(new GuanduHoldLineUsedEvent(facts.RoundNumber, source.OwnerSeat, facts.TurnNumber) { ExcludedDiscardPhaseToken = CurrentDiscardToken });
    }
    public bool IsTenthAction(long actionId) => _host.History<GuanduActualCardActionEvent>().Any(fact => fact.ActionId == actionId && fact.UseOrdinal % 10 == 0);
    public bool DamageRewardAvailable(long useFrameId) => !_host.History<GuanduDamageRewardedEvent>().Any(fact => fact.UseFrameId == useFrameId);
    public bool AllowsEvent(ModeGuanduTriggerFacts trigger)
    {
        if (trigger.SkillId != "guandu:battle-event") return true;
        if (!Active || !trigger.HasContext) return false;
        var facts = _host.CaptureFacts();
        var owner = facts.Players[trigger.OwnerSeat];
        return trigger.BindingId switch
        {
            "tenth-use" => CurrentEvent == GuanduBattleEvent.TenWinsTenLosses && trigger.HasTenthUse,
            "damage-draw" => CurrentEvent == GuanduBattleEvent.GrainShortage && trigger.Amount > 0 && trigger.HasDamageRewardUse,
            "turn-duel" => CurrentEvent == GuanduBattleEvent.SlayTheGenerals && trigger.OwnerSeat == facts.CurrentSeat,
            "arrogance" => CurrentEvent == GuanduBattleEvent.Arrogance && trigger.OwnerSeat == facts.CurrentSeat,
            "arrogance-discard" => CurrentEvent == GuanduBattleEvent.Arrogance && trigger.OwnerSeat == facts.CurrentSeat &&
                owner.HandCount + owner.EquipmentCount > 0 && facts.Players.Where(player => player.IsAlive && player.Seat != trigger.OwnerSeat).All(player => player.Hp < owner.Hp),
            _ => false
        };
    }
    public SkillProgramStepOutcome ExecuteEvent(long frameId, int ownerSeat, SkillProgramEffectOp operation)
    {
        if (!Active) throw new InvalidOperationException("A Guandu event lost its exact once-issued program.");
        if (operation == SkillProgramEffectOp.ResolveGuanduRepeat) { _flow.IssueRepeat(frameId); return SkillProgramStepOutcome.Continue; }
        var rewarding = operation == SkillProgramEffectOp.RewardGuanduDamage;
        var useId = rewarding ? _flow.DamageRewardUse(frameId) : null;
        if (rewarding && useId is null) throw new InvalidOperationException("Grain shortage cannot reward a card twice.");
        if (!rewarding && operation != SkillProgramEffectOp.ResolveGuanduArrogance) throw new InvalidOperationException("Unknown Guandu event operation.");
        var players = _host.CaptureFacts().Players;
        if (!rewarding && players.Any(player => player.IsAlive && player.HandCount > players[ownerSeat].HandCount)) return SkillProgramStepOutcome.Continue;
        _flow.IssueFlow(frameId, operation, useId);
        if (rewarding) { _flow.BeginRewardDraw(frameId, ownerSeat); return SkillProgramStepOutcome.AwaitChild; }
        return _flow.LoseOwnerHp(frameId) || ResumeEvent(frameId) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }
    public bool ResumeEvent(long frameId)
    {
        if (_flow.Current(frameId) is not { } cursor) return false;
        if (cursor.Receipt.Operation == SkillProgramEffectOp.RewardGuanduDamage && cursor.HasNativeDraw)
            throw new InvalidOperationException("Guandu draw cannot pass an unreturned draw request.");
        if (_flow.DrainChildren(frameId)) return true;
        _flow.ClearFlow(frameId);
        return false;
    }
}
