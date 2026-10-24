using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Owns successive-encounter decisions; every mutation uses the native command and frame owners.</summary>
internal class BossEncounterFeature : IBossEncounterFlowFeature, IMatchModeRuntimeHookProvider
{
    protected readonly ContentModeDefinition Mode;
    protected readonly IModeEncounterHost Host;

    internal BossEncounterFeature(ContentModeDefinition mode, IModeEncounterHost host)
    {
        Mode = mode;
        Host = host;
        RuntimeHooks = Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 30, TryAdvanceStage)]);
    }

    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }

    protected int StageIndex => Host.History<BossActivityStageStartedEvent>()
        .Select(fact => fact.StageIndex).DefaultIfEmpty(0).Last();

    protected static int BossSeat(ModeEncounterFacts facts) => (Math.Max(0, facts.HumanSeat) + 3) % facts.PlayerCount;

    private bool TryAdvanceStage()
    {
        var facts = Host.CaptureFacts();
        var activity = Mode.BossActivity!;
        var boss = facts.Players[BossSeat(facts)];
        if (!facts.SetupComplete || facts.HasWinner || boss.IsAlive || facts.ResolutionCount != 0 ||
            facts.PendingDecisionKind is { } decision && decision != DecisionKind.PlayCard) return false;
        var stageIndex = StageIndex;
        if (Mode.HulaoActivity is not null && stageIndex == 0 && boss.MaxHp > 0) return false;
        if (stageIndex + 1 >= activity.Stages.Count || Mode.HulaoActivity is not null && boss.MaxHp == 0)
        {
            Host.ClearDecision();
            Host.DeclareTeamWinner(activity.PlayerTeamId);
            Host.PublishState();
            return true;
        }
        if (facts.CurrentSeat == boss.Seat && facts.Phase is not (TurnPhase.NotStarted or TurnPhase.Finished))
        {
            // A counterattack death completes its entire actual turn before
            // a fresh character may occupy the same seat.
            Host.EndCurrentTurn();
            return true;
        }

        var nextIndex = stageIndex + 1;
        var stage = activity.Stages[nextIndex];
        var generalId = stage.GeneralPoolIds[Host.NextRandom(stage.GeneralPoolIds.Count)];
        var general = Host.General(generalId);
        Host.ReplaceCombatant(new(boss.Seat, boss.GeneralId, generalId, PreserveCurrentState: false));
        Host.ClearDecision();
        Host.PublishFact(new BossActivityStageStartedEvent(nextIndex, boss.Seat, generalId));
        Host.DrawInitialCards(boss.Seat, stage.InitialHandSize);
        Host.Log("ActivityStageStarted", $"第 {nextIndex + 1}/{activity.Stages.Count} 关：{stage.Name}，{general.Name}登场。", boss.Seat);
        Host.PublishState();
        return true;
    }
}
