#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoXianFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::CardGame.Core.EngineStates.CoreGameEnginePublicPileSourcesState gameEnginePublicPileSourcesState) : ICaoXianFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public int Keep(int owner, int card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[owner]);
    public bool Friendly(int owner, int other) => owner == other || (aiBrainsState.ReferenceToAiBrains.TryGetValue(owner, out var brain) ? brain.ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : matchParticipantsState.ReferenceToPlayers[owner].TeamRevealed && matchParticipantsState.ReferenceToPlayers[other].TeamRevealed && matchParticipantsState.ReferenceToPlayers[owner].TeamId is { } team && matchParticipantsState.ReferenceToPlayers[other].TeamId == team);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<PublicPersistentPileSource> Piles(int owner, string consumerInstance) => Array.AsReadOnly(getCoreGameEnginePublicPileSourcesRuntime().ReferencedPublicPileSources(owner, CaoXianRuntime.Lingxi, consumerInstance).ToArray());
    public IReadOnlyList<CaoXianCard> PileCards(PublicPersistentPileSource pile) => Array.AsReadOnly(getCoreGameEnginePublicPileSourcesRuntime().PublicPileSources(pile.OwnerSeat).Any(p => p == pile) ? getCoreGameEnginePublicPileSourcesRuntime().PublicPileCards(pile).Select(c => new CaoXianCard(c.Id, c.Kind, c.Suit, pile.Location, c.DisplayName)).ToArray() : []);
    public IReadOnlyList<CaoXianCard> Owned(int owner, bool handOnly, bool discard) => !Alive(owner) ? [] : Array.AsReadOnly((handOnly ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => getCardZoneStore().CardsAt(new(z, owner)).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(owner, c, new(z, owner), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (!discard || !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(owner, c, new(z, owner), OwnedCardMoveIntent.Discard))).Select(c => new CaoXianCard(c.Id, c.Kind, c.Suit, new(z, owner), c.DisplayName))).ToArray());
    public CaoXianCompletedUse? Completed(ProgramSkillWindowContext w, int owner)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != owner || w.CardUse is not { } info || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActorSeat != owner || info.CardActionId != p.Action.ActionId || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].OwnerSeat != owner)
            return null;
        return new(info.ParentCardUseFrameId, p.Action.ActionId);
    }

    public PublicPersistentPileSource? RemovedPile(ProgramSkillWindowContext w, string instance)
    {
        if (w.Window != SkillProgramTriggerWindow.CardsMoved || w.MovementBatch is not { } batch || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not CardsMovedTriggerWindowFrame p || p.Id != batch.Id || !p.Batch.Movements.SequenceEqual(batch.Movements) || !gameEnginePublicPileSourcesState.ReferenceToPublicPersistentPiles.TryGetValue((w.OwnerSeat, CaoXianRuntime.Lingxi, instance), out var pile))
            return null;
        return batch.Movements.Any(m => m.From == pile.Location && m.To != m.From && cardMovementJournalState.ReferenceToCardMovements.Contains(m)) ? pile : null;
    }

    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (at < 1 || getFrameStore()[at - 1].Id != w.ParentFrameId)
            return false;
        if (getFrameStore()[at - 1] is ProgramCardTriggerWindowFrame card)
        {
            if (card.CandidateIndex < 0 || card.CandidateIndex >= card.Candidates.Count || w.CardUse?.CardActionId != card.Action.ActionId || Completed(w, f.OwnerSeat)is null)
                return false;
            var c = card.Candidates[card.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        var candidate = getFrameStore()[at - 1] switch
        {
            PlayPhaseStartingBoundaryFrame p when w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            CardsMovedTriggerWindowFrame p when w.Window == SkillProgramTriggerWindow.CardsMoved && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            _ => null
        };
        return candidate is not null && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    }
}
