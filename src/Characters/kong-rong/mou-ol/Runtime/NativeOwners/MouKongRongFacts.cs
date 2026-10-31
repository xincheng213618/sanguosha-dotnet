#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouKongRongFacts(Func<ContentRegistry> getContentRegistry, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterKongRongMouOlRuntime>> getCharacterKongRongMouOlRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime) : IMouKongRongFacts
{
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool GameOver => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public bool Friendly(int actor, int target) => aiBrainsState.ReferenceToAiBrains[actor].ScoreProgramTarget(createSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false)) > 0;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int Xian(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MouKongRongXian);
    public IReadOnlyList<int> Living => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    public IReadOnlyList<T> History<T>()
        where T : IGameEvent => Array.AsReadOnly(getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>().ToArray());
    public bool ParentMatches(ProgramSkillFrame f) => getCharacterKongRongMouOlRuntime().Value.MouKrParentMatches(f);
    public CardActionContext? Action(ProgramSkillFrame f) => getFrameStore().SingleOrDefault(p => p.Id == f.WindowContext?.ParentFrameId)is ProgramCardTriggerWindowFrame w ? w.Action : null;
    public MouKrZhengyiStartedEvent? Damage(ProgramSkillFrame f) => getFrameStore().SingleOrDefault(p => p.Id == f.WindowContext?.ParentFrameId)is BeforeDamageProgramWindowFrame { Nature: DamageNature.Normal, Amount: > 0 } w && getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is { } a && a.TargetSeat == w.TargetSeat ? new(f.Id, MouKongRongRuntime.Source(f), f.GameplayHash, w.Id, a.ResolutionId, w.TargetSeat, w.Amount) : null;
    public IReadOnlyList<LegalAction> Actions(ProgramSkillFrame f, int seat) => getCharacterKongRongMouOlRuntime().Value.MouKrActions(f, seat);
    public bool DraftValid(LegalAction a, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => cards.Count >= a.MinCardCount && cards.Count <= a.MaxCardCount && cards.Distinct().Count() == cards.Count && targets.Count >= a.MinTargetCount && targets.Count <= a.MaxTargetCount && targets.Distinct().Count() == targets.Count && cards.All(a.SelectableCardIds.Contains) && targets.All(a.SelectableTargetSeats.Contains) && (a.SelectableTargetSequences is null || a.SelectableTargetSequences.Any(t => t.SequenceEqual(targets)));
}
