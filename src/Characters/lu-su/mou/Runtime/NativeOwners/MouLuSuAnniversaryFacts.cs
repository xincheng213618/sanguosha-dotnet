#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouLuSuAnniversaryFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CoreSnapshotValueProjectionEntries> getCoreSnapshotValueProjectionRuntime, Func<CardZoneStore> getCardZoneStore, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreDamageObserverCursorQueriesEntries> getCoreDamageObserverCursorQueriesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuSuMouRuntime>> getCharacterLuSuMouRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime) : IMouLuSuAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<CardSnapshot> Hand(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Select(getCoreSnapshotValueProjectionRuntime().ToSnapshot).ToArray());
    public bool InHand(int seat, int id) => getCardZoneStore().GetLocation(id) == CardLocation.Hand(seat);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public bool Yin(int owner, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(owner, MouLuSuAnniversaryRuntime.Mengmou, instance, "yin");
    public ActualDiscardRecoveryPhaseKey? Phase() => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public bool Parent(ProgramSkillFrame f)
    {
        var i = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (i < 1 || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || getFrameStore()[i - 1].Id != w.ParentFrameId)
            return false;
        return getFrameStore()[i - 1] switch
        {
            CardsMovedTriggerWindowFrame p => p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            DamageTriggerWindowFrame p => getCoreDamageObserverCursorQueriesRuntime().DamageProgramMatchesWindow(f, p),
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }

    public int? Peer(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterLuSuMouRuntime().Value.MouLuSuGainPeer(c, w);
    public (int Actor, int Amount, long UseId)? Damage(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.Amount <= 0 || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not DamageTriggerWindowFrame p || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].ToProgramCandidate() != c || getFrameStore().SingleOrDefault(x => x.Id == p.ParentFrameId)is not DamageFrame damage || w.DamageFrameId != damage.Id || w.Amount != damage.Amount || w.SourceSeat != damage.SourceSeat || w.TargetSeat != damage.TargetSeat || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(damage.ParentFrameId)is not { MouLuSuAnniversary: { } o } use || o.Source.OwnerSeat != c.OwnerSeat || o.Source.SkillInstanceId != c.SkillInstanceId || o.Source.SkillId != c.SkillId || o.GameplayHash != c.GameplayHash || damage.SourceSeat != o.Actor || !getCharacterLuSuMouRuntime().Value.MouLuSuNativeMatches(use))
            return null;
        return (o.Actor, w.Amount, use.Id);
    }

    public IReadOnlyList<LegalAction> Uses(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().Value.MouLuSuUseActions(f);
    public IReadOnlyList<MouLuSuAnniversaryPlayedPlan> Plays(ProgramSkillFrame f) => getCharacterLuSuMouRuntime().Value.MouLuSuPlayedPlans(f);
    public bool Declaration(MouLuSuAnniversaryPlayedPlan plan) => plan.Source is { } source && getCoreGameEnginePlayCardHistoryRuntime().ViewAsRule(source)?.DeclarationValidation is not null;
}
