#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangXiuAnniversaryFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime) : IMouZhangXiuAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public int Keep(int owner, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[owner]);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool Largest(int seat) => Alive(seat) && matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).All(p => getCorePhysicalOwnedCardQueriesRuntime().GetHand(p).Count <= getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count);
    public int PlayActor(ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is PlayPhaseStartingBoundaryFrame p && w.SourceSeat == p.OwnerSeat && w.TargetSeat == p.OwnerSeat && actualTurnProgressState.ReferenceToCurrentSeat == p.OwnerSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play ? p.OwnerSeat : -1;
    public IReadOnlyList<int> Others(int owner) => matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner).Select(p => p.Seat).ToArray();
    public IReadOnlyList<MouZhangXiuAnniversaryCard> Entities(IEnumerable<int> ids) => Array.AsReadOnly(ids.Distinct().Select(id =>
    {
        var c = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        return new MouZhangXiuAnniversaryCard(c.Id, c.Kind, getCardZoneStore().GetLocation(id), c.DisplayName);
    }).ToArray());
    public IReadOnlyList<MouZhangXiuAnniversaryCard> Cards(int actor, int holder, bool discard) => !Alive(actor) || !Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, holder)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(actor, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (actor != holder || !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(holder, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer))).Select(c => new MouZhangXiuAnniversaryCard(c.Id, c.Kind, new(z, holder), c.DisplayName))).ToArray());
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (n < 1 || getFrameStore()[n - 1].Id != w.ParentFrameId)
            return false;
        return getFrameStore()[n - 1] switch
        {
            PlayPhaseStartingBoundaryFrame p => f.SkillId == MouZhangXiuAnniversaryRuntime.Fuxi && f.TriggerId == "other-largest-hand" && w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && PlayActor(w) == p.OwnerSeat && p.OwnerSeat != f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            TurnEndingBoundaryFrame p => f.SkillId == MouZhangXiuAnniversaryRuntime.Haoyi && f.TriggerId == "turn-damage-card-claim" && w.Window == SkillProgramTriggerWindow.TurnEnding && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            _ => false
        };
    }
}
