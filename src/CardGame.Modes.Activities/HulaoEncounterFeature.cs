using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Owns synchronous transformation qualification, cancellation choreography and the shuffle deadline.</summary>
internal sealed class HulaoEncounterFeature : BossEncounterFeature,
    IMatchModeRuleObserverProvider, IModeFlowInterruptionHandler, IModeDeckRecycleObserver
{
    internal HulaoEncounterFeature(ContentModeDefinition mode, IModeEncounterHost host) : base(mode, host)
    {
        RuleObservers = Array.AsReadOnly<ModeRuleObserverRegistration>(
        [
            ModeRuleObserverRegistration.ForEvent<IGameEvent>(ProgramRulePhase.AfterEvent, 27600, ObserveTransformation),
            ModeRuleObserverRegistration.ForEvent<TurnEndedEvent>(ProgramRulePhase.AfterEvent, 27601, EndAfterShuffleLimit)
        ]);
    }

    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    public string InterruptionHandlerId => "activity:hulao-transformation";
    private int ShuffleCount => Host.History<HulaoDeckShuffledEvent>()
        .Select(fact => fact.ShuffleNumber).DefaultIfEmpty().Last();

    public void OnDeckRecycled() => Host.PublishFact(
        new HulaoDeckShuffledEvent(ShuffleCount + 1, Host.CaptureFacts().TurnNumber));

    private void ObserveTransformation(IGameEvent fact)
    {
        var facts = Host.CaptureFacts();
        if (!facts.SetupComplete || facts.TurnNumber <= 0 || facts.HasWinner || StageIndex != 0) return;
        var boss = facts.Players[BossSeat(facts)];
        if (fact is HulaoDeckShuffledEvent { ShuffleNumber: 1 })
            Interrupt(facts, boss, HulaoTransformationCause.FirstShuffle);
        if (fact is DamageAppliedEvent or ProgramSkillHpLostEvent or MaximumHpChangedEvent or DamageReplacedWithHpLossEvent &&
            boss.IsAlive && boss.MaxHp > 0 && boss.Hp <= Mode.HulaoActivity!.TransformationHp)
            Interrupt(facts, boss, HulaoTransformationCause.HealthThreshold);
    }

    private void Interrupt(ModeEncounterFacts facts, ModeEncounterPlayerFacts boss, HulaoTransformationCause cause) =>
        throw new ModeFlowInterruption(InterruptionHandlerId,
            new TransformationRequest(facts.TurnNumber, boss.Seat, boss.GeneralId, cause));

    private void EndAfterShuffleLimit(TurnEndedEvent ended)
    {
        if (ShuffleCount < Mode.HulaoActivity!.ShuffleLimit || Host.CaptureFacts().HasWinner) return;
        Host.DeclareTeamWinner(Mode.BossActivity!.EnemyTeamId);
    }

    public void CompleteInterruption(ModeFlowTransitionRequest request)
    {
        var facts = Host.CaptureFacts();
        if (request is not TransformationRequest transformation || facts.HasWinner || StageIndex != 0 ||
            transformation.TurnNumber != facts.TurnNumber || transformation.BossSeat != BossSeat(facts) ||
            facts.Players[transformation.BossSeat].GeneralId != transformation.BossGeneralId)
            throw new InvalidOperationException("An encounter interruption lost its original transition owner.");
        var boss = facts.Players[transformation.BossSeat];
        var previousSeat = facts.CurrentSeat;
        var stage = Mode.BossActivity!.Stages[1];
        var generalId = stage.GeneralPoolIds[Host.NextRandom(stage.GeneralPoolIds.Count)];
        var general = Host.General(generalId);
        Host.ReplaceCombatant(new(boss.Seat, boss.GeneralId, generalId, PreserveCurrentState: true));
        // Publish the new stage before cleanup can recursively raise another fact.
        Host.PublishFact(new BossActivityStageStartedEvent(1, boss.Seat, generalId));
        Host.ClearDecision(resumeRunning: true);
        Host.CancelResolutionTree(frame => new HulaoResolutionCancelledEvent(frame.FrameId, frame.Kind, frame.TurnNumber));
        Host.ClearTransientBatches();
        Host.DiscardProcessingCards("activity.hulao.interrupted-processing");
        Host.FinishInterruptedTurn(previousSeat, facts.TurnNumber, "hulao-transformation",
            new HulaoTurnInterruptedEvent(facts.TurnNumber, previousSeat, transformation.Cause));

        // Preserve the established edition: health-triggered transformation
        // restarts with the vanguard; a first-shuffle transformation starts with the boss.
        var refreshed = Host.CaptureFacts();
        var firstAlly = Enumerable.Range(0, refreshed.PlayerCount)
            .Select(offset => (Math.Max(0, refreshed.HumanSeat) + offset) % refreshed.PlayerCount)
            .First(seat => seat != boss.Seat && refreshed.Players[seat].IsAlive);
        Host.BeginNaturalTurnAt(transformation.Cause == HulaoTransformationCause.FirstShuffle ? boss.Seat : firstAlly);
        Host.Log("HulaoTransformed", $"神吕布化为【{general.Name}】，当前回合中断，第二阶段开始。", boss.Seat);
        Host.PublishState();
    }

    private sealed record TransformationRequest(int TurnNumber, int BossSeat, string BossGeneralId,
        HulaoTransformationCause Cause) : ModeFlowTransitionRequest;
}
