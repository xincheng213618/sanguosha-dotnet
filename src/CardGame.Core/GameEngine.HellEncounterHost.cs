namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IHellEncounterHost CreateHellEncounterHost() => new HellEncounterHost(this);
    private IHellEncounterFeature? HellFeature => _modeRuntime.Find<IHellEncounterFeature>();

    private sealed class HellEncounterHost(GameEngine engine) : IHellEncounterHost
    {
        private readonly IModeEncounterHost _encounter = new ModeEncounterHost(engine);
        public IModeEncounterHost Encounter => _encounter;
        public bool GameStartingProgramsResolved => engine._gameStartingProgramsResolved;
        public int ActualTurnOwnerSeat => engine._turnProgression.OwnerSeat;
        public int InitialHandSize => engine._initialHandSize;
        public long NextResolutionId() => ++engine._resolutionSequence;

        public ModeNpcArrivalSourceFacts? ArrivalSource(long resolutionId)
        {
            if (engine.LifecycleCardUse(resolutionId)?.VirtualOrdinaryTrickOrigin is not { } origin ||
                engine._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ParentProgramFrameId) is not { } parent ||
                engine.CaptureNativeArrivalProgramSource(parent) is not { } source) return null;
            return source with { SkillId = origin.SkillId };
        }

        public bool TryOpenNpcGameStartingWindow(HellNpcAppearedEvent appearance)
        {
            var npc = engine._players[appearance.Seat];
            var facts = engine.CaptureProgramTriggerFacts(npc);
            var candidates = engine.CollectEligibleProgramTriggerCandidates(npc, SkillProgramTriggerWindow.GameStarting, facts).ToArray();
            if (!npc.IsAlive || candidates.Length == 0) return false;
            engine._pendingDecision = null;
            engine.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++engine._resolutionSequence, engine._currentSeat,
                SkillProgramTriggerWindow.GameStarting, candidates, ProgramLifecycleContinuation.ResumeHellAppearance, facts)
            { ParticipantFacts = new Dictionary<int, SkillProgramTriggerFacts> { [npc.Seat] = facts }, HellAppearanceId = appearance.AppearanceId });
            engine.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
            return true;
        }

        public void RestoreDormantCombatant(int seat, int maximumHp, int hp)
        {
            var player = engine._players[seat];
            player.IsAlive = player.GeneralRevealed = true;
            player.MaxHp = maximumHp;
            player.Hp = hp;
        }
    }
}
