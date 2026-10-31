#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChenLinFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CharacterChenLinMouEntries> getCharacterChenLinMouRuntime, Func<FrameStore> getFrameStore) : IMouChenLinFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public int HandCount(int s) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count;
    public bool Friendly(int owner, int other) => owner == other || (aiBrainsState.ReferenceToAiBrains.TryGetValue(owner, out var brain) ? brain.ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : matchParticipantsState.ReferenceToPlayers[owner].TeamRevealed && matchParticipantsState.ReferenceToPlayers[other].TeamRevealed && matchParticipantsState.ReferenceToPlayers[owner].TeamId is { } team && matchParticipantsState.ReferenceToPlayers[other].TeamId == team);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public string SkillName(string id) => getContentRegistry().GetSkill(id).Name;
    public string SkillDescription(string id) => getContentRegistry().GetSkill(id).Description;
    public ActualDiscardRecoveryPhaseKey? Phase() => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public bool Recorded(ActualDiscardRecoveryPhaseKey phase) => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(phase);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
    public IReadOnlyList<MouChenLinCard> He(int actor) => !Alive(actor) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, actor)).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(actor, c, new(z, actor), OwnedCardMoveIntent.Transfer)).Select(c => new MouChenLinCard(c.Id, c.Kind, new(z, actor), c.DisplayName))).ToArray());
    public IReadOnlyList<MouChenLinCard> Current(IEnumerable<int> ids) => Array.AsReadOnly(ids.Select(id =>
    {
        var c = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        return new MouChenLinCard(id, c.Kind, getCardZoneStore().GetLocation(id), c.DisplayName);
    }).ToArray());
    public IReadOnlyList<MouChenLinCard> Revealed(ProgramSkillFrame f) => Current(f.CardSetBindings.Single(b => b.Name == MouChenLinRuntime.RevealBind).CardIds);
    public MouChenLinUsePlan? Plan(ProgramSkillFrame f, int id, int target) => getCharacterChenLinMouRuntime().MouChenLinPlan(f, id, target);
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouChenLinRuntime.Yaozuo)
            return f.TriggerId is null && f.ActivationId == "invite-authors" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && History<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId && e.OwnerSeat == f.OwnerSeat) == 1;
        if (f.SkillId != MouChenLinRuntime.Zhuanwen || f.TriggerId != "ending-top-written-cards" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        return n >= 1 && getFrameStore()[n - 1] is TurnEndingBoundaryFrame p && p.Id == w.ParentFrameId && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate);
    }
}
