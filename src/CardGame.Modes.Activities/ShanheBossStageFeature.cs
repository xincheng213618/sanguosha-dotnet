using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Activities;

/// <summary>The configured second form interrupts lethal first-form damage before dying or settlement.</summary>
internal sealed class ShanheBossStageFeature : IGameModeFeature,
    IMatchModeRuleObserverProvider, IModeFlowInterruptionHandler
{
    private readonly ShanheBattleSetup _setup;
    private readonly IModeEncounterHost _host;
    private readonly IStandardBattleHost _battleHost;
    internal ShanheBossStageFeature(ShanheBattleSetup setup, IModeEncounterHost host, IStandardBattleHost battleHost)
    {
        _setup = setup; _host = host; _battleHost = battleHost;
        RuleObservers = Array.AsReadOnly<ModeRuleObserverRegistration>(
        [ModeRuleObserverRegistration.ForEvent<IGameEvent>(ProgramRulePhase.AfterEvent, 27600, ObserveLethalDamage)]);
    }

    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    public string InterruptionHandlerId => "campaign:shanhe-boss-stage";
    private int BossSeat => 1 + _setup.AllyRoster.Count + _setup.BossTransition!.EnemyIndex;
    private bool HasTransformed => _host.History<ShanheBossStageStartedEvent>().Count != 0;

    private void ObserveLethalDamage(IGameEvent fact)
    {
        var affectedSeat = fact switch
        {
            DamageAppliedEvent damage => damage.TargetSeat,
            ProgramSkillHpLostEvent lost => lost.TargetSeat,
            MaximumHpChangedEvent maximum => maximum.PlayerSeat,
            DamageReplacedWithHpLossEvent replacement => replacement.TargetSeat,
            // This native fact precedes direct zero-maximum death; the later
            // MaximumHpChanged fact is deliberately published after death.
            CardDamageReplacedWithMaximumHpLossEvent replacement => replacement.Receipt.TargetSeat,
            _ => -1
        };
        if (affectedSeat != BossSeat) return;
        var facts = _host.CaptureFacts();
        if (!facts.SetupComplete || facts.HasWinner || HasTransformed) return;
        var boss = facts.Players[BossSeat];
        if (!boss.IsAlive || boss.Hp > 0) return;
        if (facts.TurnNumber == 0)
        {
            // No actual turn exists to retire. Only an effective Dongdang
            // instance may cancel this startup; other forms retain its parents.
            var transition = _setup.BossTransition!;
            EnterSecondForm(facts);
            if (_battleHost.ActiveShanheDongdangSource(BossSeat) is { } source)
                throw new ModeFlowInterruption(InterruptionHandlerId, new OpeningDongdangRequest(boss.GeneralId, source));
            DealSecondFormCards(transition);
            _host.Log("ShanheBossTransformed", $"首领在开局进入第二阶段【{_host.General(transition.GeneralId).Name}】。", BossSeat);
            _host.PublishState();
            return;
        }
        throw new ModeFlowInterruption(InterruptionHandlerId, new StageRequest(facts.TurnNumber, boss.GeneralId));
    }

    public void CompleteInterruption(ModeFlowTransitionRequest request)
    {
        var facts = _host.CaptureFacts();
        var boss = facts.Players[BossSeat];
        if (request is OpeningDongdangRequest opening)
        {
            if (facts.TurnNumber != 0 || facts.HasWinner || !boss.IsAlive || boss.Hp <= 0 ||
                _host.History<ShanheBossStageStartedEvent>() is not [{ TurnNumber: 0 } entered] ||
                entered.BossSeat != BossSeat || entered.PreviousGeneralId != opening.PreviousGeneralId ||
                entered.GeneralId != boss.GeneralId || _setup.BossTransition!.GeneralId != boss.GeneralId ||
                _battleHost.ActiveShanheDongdangSource(BossSeat) != opening.Source)
                throw new InvalidOperationException("Opening Dongdang lost its actual newly entered skill instance.");
            InterruptForSecondForm(facts, opening.PreviousGeneralId, opening.Source);
            return;
        }
        if (request is not StageRequest stage || stage.TurnNumber <= 0 || facts.HasWinner || HasTransformed ||
            stage.TurnNumber != facts.TurnNumber || stage.GeneralId != boss.GeneralId || !boss.IsAlive || boss.Hp > 0)
            throw new InvalidOperationException("A Shanhe boss transition lost its lethal first-form owner.");
        var transition = _setup.BossTransition!;
        EnterSecondForm(facts);
        InterruptForSecondForm(facts, stage.GeneralId, _battleHost.ActiveShanheDongdangSource(BossSeat));
    }

    private void InterruptForSecondForm(ModeEncounterFacts facts, string previousGeneralId, CardConversionSource? source)
    {
        var transition = _setup.BossTransition!;
        if (source is not null)
        {
            if (source.SkillId != ShanheSourceDongdangRules.SkillId || source.BindingId != ShanheSourceDongdangRules.BindingId ||
                source.OwnerSeat != BossSeat || _battleHost.ActiveShanheDongdangSource(BossSeat) != source ||
                _host.History<ShanheSourceDongdangActivatedEvent>().Count != 0)
                throw new InvalidOperationException("Dongdang cannot consume another source or enter twice.");
            _host.PublishFact(new ShanheSourceDongdangActivatedEvent(BossSeat, source, previousGeneralId,
                transition.GeneralId, facts.CurrentSeat, facts.TurnNumber));
        }
        _host.ClearDecision(resumeRunning: true);
        _host.CancelResolutionTree(frame => new ShanheBossResolutionCancelledEvent(frame.FrameId, frame.Kind, frame.TurnNumber));
        _host.ClearTransientBatches();
        _host.DiscardProcessingCards("campaign.shanhe.boss-interrupted-processing");
        if (facts.TurnNumber > 0)
            _host.FinishInterruptedTurn(facts.CurrentSeat, facts.TurnNumber, "shanhe-boss-stage",
                new ShanheBossTurnInterruptedEvent(facts.TurnNumber, facts.CurrentSeat, BossSeat));
        DealSecondFormCards(transition);
        _host.BeginNaturalTurnAt(BossSeat);
        _host.Log("ShanheBossTransformed", $"首领进入第二阶段【{_host.General(transition.GeneralId).Name}】，当前回合中断，由首领开始新的自然回合。", BossSeat);
        _host.PublishState();
    }

    private void EnterSecondForm(ModeEncounterFacts facts)
    {
        var boss = facts.Players[BossSeat];
        var transition = _setup.BossTransition!;
        _host.ReplaceCombatant(new(BossSeat, boss.GeneralId, transition.GeneralId, PreserveCurrentState: true,
            MaximumHpOverride: transition.MaxHp, HpOverride: transition.Hp, ArmorOverride: transition.Combatant?.InitialArmor));
        // Commit the once-only marker before opening draws or cleanup publish another rule event.
        _host.PublishFact(new ShanheBossStageStartedEvent(BossSeat, boss.GeneralId, transition.GeneralId,
            facts.TurnNumber, transition.MaxHp, transition.Hp));
    }

    private void DealSecondFormCards(ShanheBossTransition transition)
    {
        if (transition.DrawCards != 0) _host.DrawInitialCards(BossSeat, transition.DrawCards);
        foreach (var card in transition.StartingEquipment ?? [])
            _battleHost.PlaceStartingCard(card, BossSeat, equipment: true);
    }

    private sealed record StageRequest(int TurnNumber, string GeneralId) : ModeFlowTransitionRequest;
    private sealed record OpeningDongdangRequest(string PreviousGeneralId, CardConversionSource Source) : ModeFlowTransitionRequest;
}
