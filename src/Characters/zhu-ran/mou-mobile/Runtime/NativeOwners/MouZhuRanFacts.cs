#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhuRanFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : IMouZhuRanFacts
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
    public int KeepValue(int seat, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[seat]);
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
        return n > 0 && getFrameStore()[n - 1] is TurnEndingBoundaryFrame parent && parent.Id == w.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }
}
