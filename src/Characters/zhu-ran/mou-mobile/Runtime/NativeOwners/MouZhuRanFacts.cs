#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhuRanFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<ContentRegistry> getContentRegistry, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<FrameStore> getFrameStore) : IMouZhuRanFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public ActualDiscardRecoveryPhaseKey? Phase => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public IReadOnlyList<int> Seats => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(p => p.Seat).ToArray());

    public IReadOnlyList<int> ActionOrder(int from, IEnumerable<int> seats) => Array.AsReadOnly(seats.Distinct().OrderBy(s => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, s)).ToArray());
    public MouZhuRanPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]));
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public SkillProgram? Program(string skill) => getContentRegistry().Skills.GetValueOrDefault(skill)?.Program;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public string CardLabel(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName;
    public int KeepValue(int seat, int id) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[seat]);
    public bool Friendly(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public IReadOnlyList<MouZhuRanCard> Discardable(int seat) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, seat)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard)).Select(c => new MouZhuRanCard(c.Id, new(z, seat), getCoreGameEngineProgramCardPoliciesRuntime().GetProgramEffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], c)))).ToArray());
    public bool ExactStart(ProgramSkillFrame f, SkillProgramEffectOp operation)
    {
        if (f.InstructionIndex < 1 || Program(f.SkillId)?.GameplayHash != f.GameplayHash || getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(f)?.Op != operation || !getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return false;
        if (operation == SkillProgramEffectOp.ResolveMouZhuRanZhenwei)
            return f.SkillId == MouZhuRanRuntime.Zhenwei && f.ActivationId == "sealed-discard" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats is [var target] && target != f.OwnerSeat && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) && matchParticipantsState.ReferenceToPlayers[target].IsAlive && Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p) && !History<MouZhuRanStartedEvent>().Any(x => x.Source.OwnerSeat == f.OwnerSeat && x.Phase == p && x.Operation == operation);
        return operation == SkillProgramEffectOp.ResolveMouZhuRanHeyuan && f.SkillId == MouZhuRanRuntime.Heyuan && f.TriggerId == "repeat-last" && f.ActivationId == f.TriggerId && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && NativeParent(f);
    }

    public bool NativeParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || w.OwnerSeat != f.OwnerSeat || w.SourceSeat != f.OwnerSeat || w.TargetSeat != f.OwnerSeat || actualTurnProgressState.ReferenceToCurrentSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(p => p.Id == f.Id);
        return n > 0 && getFrameStore()[n - 1] is TurnEndingBoundaryFrame parent && parent.Id == w.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } c && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }
}
