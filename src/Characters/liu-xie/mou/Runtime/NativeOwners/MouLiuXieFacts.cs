#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouLiuXieFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries> getCoreGameEngineLordSkillProjectionsRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime) : IMouLiuXieFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;

    public int TableStepSeat(int from, int offset) => getCoreGameEngineTableOrderRuntime().TableStepSeat(from, offset);
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public ActualDiscardRecoveryPhaseKey? Phase => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public bool Qun(int seat) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(matchParticipantsState.ReferenceToPlayers[seat]) == "qun";
    public bool LordQualified(int seat) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[seat], MouLiuXieComposition.Tiancheng) && getCoreGameEngineLordSkillProjectionsRuntime().HasSkillRoleQualification(matchParticipantsState.ReferenceToPlayers[seat], MouLiuXieComposition.Tiancheng, null, Role.Lord);
    public bool Friendly(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public string CardLabel(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public IReadOnlyList<int> Discardable(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray());
    public int KeepValue(int seat, int id) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[seat]);
    public SkillProgram? Program(string skill) => getContentRegistry().Skills.GetValueOrDefault(skill)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool ExactStart(ProgramSkillFrame f) => f.SkillId == MouLiuXieComposition.Zhanban && f.ActivationId == "adjust-all-hands" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && f.InstructionIndex == 1 && getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.ResolveMouLiuXieZhanban && Program(f.SkillId)?.GameplayHash == f.GameplayHash && getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p) && !History<MouLiuXieStartedEvent>().Any(x => x.Source.OwnerSeat == f.OwnerSeat && x.Phase == p);
    public bool ChenshengParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouLiuXieComposition.Chensheng || f.TriggerId != "other-actual-turn-end" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } w || w.OwnerSeat != f.OwnerSeat || Program(f.SkillId)?.GameplayHash != f.GameplayHash || actualTurnProgressState.ReferenceToCurrentSeat == f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        return n > 0 && getFrameStore()[n - 1] is DeferredTurnEndFrame parent && parent.Id == w.ParentFrameId && parent.OwnerSeat == actualTurnProgressState.ReferenceToCurrentSeat && parent.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && w.SourceSeat == parent.OwnerSeat && w.TargetSeat == parent.OwnerSeat && getCoreGameEngineAfterTurnEndedProgramsRuntime().IsActualAfterTurnEndedParent(parent) && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, getCoreGameEngineAfterTurnEndedProgramsRuntime().AfterTurnEndedCandidate(parent));
    }
}
