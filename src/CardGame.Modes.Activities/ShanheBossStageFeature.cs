using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Activities;

/// <summary>The configured second form interrupts lethal first-form damage before dying or settlement.</summary>
internal sealed class ShanheBossStageFeature : IGameModeFeature,
    IMatchModeRuleObserverProvider, IModeFlowInterruptionHandler
{
    private readonly ShanheBattleSetup _setup;
    private readonly IModeEncounterHost _host;
    internal ShanheBossStageFeature(ShanheBattleSetup setup, IModeEncounterHost host)
    {
        _setup = setup; _host = host;
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
            // No actual turn exists to retire. Keep the paid opening draw and
            // remaining GameStarting candidates on their original frames.
            var transition = _setup.BossTransition!;
            EnterSecondForm(facts);
            if (transition.DrawCards != 0) _host.DrawInitialCards(BossSeat, transition.DrawCards);
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
        if (request is not StageRequest stage || stage.TurnNumber <= 0 || facts.HasWinner || HasTransformed ||
            stage.TurnNumber != facts.TurnNumber || stage.GeneralId != boss.GeneralId || !boss.IsAlive || boss.Hp > 0)
            throw new InvalidOperationException("A Shanhe boss transition lost its lethal first-form owner.");
        var transition = _setup.BossTransition!;
        EnterSecondForm(facts);
        _host.ClearDecision(resumeRunning: true);
        _host.CancelResolutionTree(frame => new ShanheBossResolutionCancelledEvent(frame.FrameId, frame.Kind, frame.TurnNumber));
        _host.ClearTransientBatches();
        _host.DiscardProcessingCards("campaign.shanhe.boss-interrupted-processing");
        _host.FinishInterruptedTurn(facts.CurrentSeat, facts.TurnNumber, "shanhe-boss-stage",
            new ShanheBossTurnInterruptedEvent(facts.TurnNumber, facts.CurrentSeat, BossSeat));
        if (transition.DrawCards != 0) _host.DrawInitialCards(BossSeat, transition.DrawCards);
        _host.BeginNaturalTurnAt(BossSeat);
        _host.Log("ShanheBossTransformed", $"首领进入第二阶段【{_host.General(transition.GeneralId).Name}】，当前回合中断，由首领开始新的自然回合。", BossSeat);
        _host.PublishState();
    }

    private void EnterSecondForm(ModeEncounterFacts facts)
    {
        var boss = facts.Players[BossSeat];
        var transition = _setup.BossTransition!;
        _host.ReplaceCombatant(new(BossSeat, boss.GeneralId, transition.GeneralId, PreserveCurrentState: true,
            MaximumHpOverride: transition.MaxHp, HpOverride: transition.Hp));
        // Commit the once-only marker before opening draws or cleanup publish another rule event.
        _host.PublishFact(new ShanheBossStageStartedEvent(BossSeat, boss.GeneralId, transition.GeneralId,
            facts.TurnNumber, transition.MaxHp, transition.Hp));
    }

    private sealed record StageRequest(int TurnNumber, string GeneralId) : ModeFlowTransitionRequest;
}
