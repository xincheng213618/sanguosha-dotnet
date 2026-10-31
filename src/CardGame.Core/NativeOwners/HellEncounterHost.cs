#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HellEncounterHost(global::CardGame.Core.NativeOwners.ModeEncounterHostFactory createModeEncounterHost, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries> getCoreGameEngineHellActivitiesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime) : IHellEncounterHost
{
    private readonly IModeEncounterHost _encounter = createModeEncounterHost();
    public IModeEncounterHost Encounter => _encounter;
    public bool GameStartingProgramsResolved => matchSetupState.ReferenceToGameStartingProgramsResolved;
    public int ActualTurnOwnerSeat => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int InitialHandSize => matchDefinitionState.ReferenceToInitialHandSize;

    public long NextResolutionId() => ++getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
    public ModeNpcArrivalSourceFacts? ArrivalSource(long resolutionId)
    {
        if (getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(resolutionId)?.VirtualOrdinaryTrickOrigin is not { } origin || getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ParentProgramFrameId)is not { } parent || getCoreGameEngineHellActivitiesRuntime().CaptureNativeArrivalProgramSource(parent)is not { } source)
            return null;
        return source with
        {
            SkillId = origin.SkillId
        };
    }

    public bool TryOpenNpcGameStartingWindow(HellNpcAppearedEvent appearance)
    {
        var npc = matchParticipantsState.ReferenceToPlayers[appearance.Seat];
        var facts = getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(npc);
        var candidates = getCoreGameEngineProgramLifecycleRuntime().CollectEligibleProgramTriggerCandidates(npc, SkillProgramTriggerWindow.GameStarting, facts).ToArray();
        if (!npc.IsAlive || candidates.Length == 0)
            return false;
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = null;
        getCoreGameEngineRuntimeRuntime().PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence, actualTurnProgressState.ReferenceToCurrentSeat, SkillProgramTriggerWindow.GameStarting, candidates, ProgramLifecycleContinuation.ResumeHellAppearance, facts) { ParticipantFacts = new Dictionary<int, SkillProgramTriggerFacts> { [npc.Seat] = facts }, HellAppearanceId = appearance.AppearanceId });
        getCoreGameEngineRuntimeRuntime().AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    public void RestoreDormantCombatant(int seat, int maximumHp, int hp)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        player.IsAlive = player.GeneralRevealed = true;
        player.MaxHp = maximumHp;
        player.Hp = hp;
    }
}
