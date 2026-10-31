#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuLieFacts(EngineStates.CoreActualRoundProgressState actualRoundProgressState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CharacterHuLieMouEntries> getCharacterHuLieMouRuntime, Func<FrameStore> getFrameStore) : IMouHuLieFacts
{
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public int HandCount(int s) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count;
    public int Keep(int actor, int id) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[actor]);
    public bool Friendly(int actor, int target) => actor == target || (aiBrainsState.ReferenceToAiBrains.TryGetValue(actor, out var brain) ? brain.ScoreProgramTarget(createSnapshot(actor), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : matchParticipantsState.ReferenceToPlayers[actor].TeamRevealed && matchParticipantsState.ReferenceToPlayers[target].TeamRevealed && matchParticipantsState.ReferenceToPlayers[actor].TeamId is { } t && matchParticipantsState.ReferenceToPlayers[target].TeamId == t);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<int> ActionOrder(IEnumerable<int> seats) => Array.AsReadOnly(seats.Distinct().OrderBy(s => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(actualTurnProgressState.ReferenceToCurrentSeat, s)).ToArray());
    public IReadOnlyList<MouHuLieCard> Owned(int owner, bool handOnly) => !Alive(owner) ? [] : Array.AsReadOnly((handOnly ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(z => getCardZoneStore().CardsAt(new(z, owner)).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(owner, c, new(z, owner), OwnedCardMoveIntent.Transfer)).Select(c => new MouHuLieCard(c.Id, c.Kind, new(z, owner), c.DisplayName))).ToArray());
    public MouHuLieCard Current(int id)
    {
        var c = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        return new(id, c.Kind, getCardZoneStore().GetLocation(id), c.DisplayName);
    }

    public MouHuLieYu? ActiveYu(int owner, string instance) => getCharacterHuLieMouRuntime().CurrentMouHuLieYu(owner, instance);
    public MouHuLieYu? UsedYu(ProgramSkillWindowContext w, int owner, string instance)
    {
        if (w.Window != SkillProgramTriggerWindow.DiscardPileReceived || w.MovementBatch is not { } b || w.MovementIndex is not { } i || i < 0 || i >= b.Movements.Count || ActiveYu(owner, instance)is not { } yu)
            return null;
        var m = b.Movements[i];
        return getCharacterHuLieMouRuntime().IsMouHuLieUsedDiscard(b, m, yu) ? yu : null;
    }

    public int? YitouTarget(ProgramSkillWindowContext w, int owner)
    {
        if (w.Window != SkillProgramTriggerWindow.PlayPhaseStarting || w.TargetSeat is not { } target || target == owner || !Alive(target) || actualTurnProgressState.ReferenceToCurrentSeat != target || getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Count != matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Max(p => getCorePhysicalOwnedCardQueriesRuntime().GetHand(p).Count))
            return null;
        return target;
    }

    public MouHuLieDamageEvent? Damage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterHuLieMouRuntime().MouHuLieDamageReceipt(c, w);
    public bool CanSlash(ProgramSkillFrame f) => getCharacterHuLieMouRuntime().CanMouHuLieSlash(f);
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (at < 1 || getFrameStore()[at - 1].Id != w.ParentFrameId)
            return false;
        var candidate = getFrameStore()[at - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            PlayPhaseStartingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            CardsMovedTriggerWindowFrame p when p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            _ => null
        };
        if (candidate is not null)
            return EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        return w.Window == SkillProgramTriggerWindow.AfterDamageApplied && Damage(new(f.OwnerSeat, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex), w)is not null;
    }
}
