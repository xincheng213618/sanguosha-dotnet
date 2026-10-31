#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSimaShiFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime) : IMouSimaShiFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public ActualDiscardRecoveryPhaseKey? Phase() => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public IReadOnlyList<MouSimaShiDeckCard> Deck() => Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DrawPile).Select(c => new MouSimaShiDeckCard(c.Id, c.Rank)).ToArray());
    public IReadOnlyList<MouSimaShiCard> Entities(IEnumerable<int> ids) => Array.AsReadOnly(ids.Distinct().Select(id =>
    {
        var p = getCardZoneStore().GetLocation(id);
        return new MouSimaShiCard(id, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).Kind, p);
    }).ToArray());
    public bool Quarantined(MouSimaShiRemovedEvent removed) => getCardZoneStore().GetLocation(removed.CardId) == CardLocation.OutsideGame && cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(m => m.CardId == removed.CardId)is { } actual && actual.Sequence == removed.Sequence && actual.To == CardLocation.OutsideGame && getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<MouSimaShiMovedEvent>().Count(m => m.DebtId == removed.DebtId && m.CardId == removed.CardId && m.Sequence == actual.Sequence && m.Operation == SkillProgramEffectOp.RemoveMouSimaShiChenlve && m.To == actual.To) == 1;
    public MouSimaShiClaimIssuedEvent? Due(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterSimaShiMouRuntime().MouSimaShiDue(c, w);
    public MouSimaShiUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || w.CardUse is not { } info || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.Type != CardActionType.Use || p.Action.ActionId != info.CardActionId || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(info.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != p.Action.ActionId)
            return null;
        var actual = p.Candidates[p.CandidateIndex];
        if (actual.OwnerSeat != c.OwnerSeat || actual.SkillId != c.SkillId || actual.TriggerId != c.BindingId || actual.SkillInstanceId != c.SkillInstanceId || actual.GameplayHash != c.GameplayHash)
            return null;
        return new(use.Id, a.ActionId, a.ActorSeat, Array.AsReadOnly(use.TargetSeats.ToArray()));
    }

    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouSimaShiRuntime.Chenlve && f.ActivationId == "claim-dead-soldiers" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.SkillId == f.SkillId && s.OwnerSeat == f.OwnerSeat && s.ActivationId == f.ActivationId) == 1;
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (n < 1 || getFrameStore()[n - 1].Id != w.ParentFrameId)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex);
        if (f.SkillId == MouSimaShiRuntime.Chenlve)
            return Due(candidate, w)is not null;
        if (f.SkillId == MouSimaShiRuntime.Zhenrao)
            return Use(candidate, w)is not null;
        return getFrameStore()[n - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            TurnEndingBoundaryFrame p => p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate),
            _ => false
        };
    }
}
