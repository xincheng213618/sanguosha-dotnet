#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuAnniversaryFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CharacterChengYuMouEntries> getCharacterChengYuMouRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<FrameStore> getFrameStore, Func<CardZoneStore> getCardZoneStore) : IMouChengYuAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public int LivingCount => matchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive);
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public bool Enabled => getCharacterChengYuMouRuntime().HasMouChengYuAnniversary;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public bool OwnPlay(int s) => s == actualTurnProgressState.ReferenceToCurrentSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public CardKind CardName(CardKind k) => EngineOwners.CoreGameEngineNamedAmbushRuntime.AmbushCardName(k);
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<ProgramTriggerCandidate> Candidates(int owner, SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[owner], window);
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouChengYuAnniversaryRuntime.Gaojian && f.ActivationId == "reveal-and-advise" && f.WindowContext is null && History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId) == 1;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (at < 1 || f.SkillId != MouChengYuAnniversaryRuntime.Shizha || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        if (f.TriggerId == "hp-change" && getFrameStore()[at - 1] is HpChangedTriggerWindowFrame hp && w.Window == SkillProgramTriggerWindow.AfterHealthChanged && w.HpChange is { } change && hp.Id == w.ParentFrameId && (hp.Change.Id, hp.Change.ParentFrameId, hp.Change.SourceSeat, hp.Change.TargetSeat, hp.Change.Kind, hp.Change.Amount, hp.Change.HpBefore, hp.Change.HpAfter) == (change.Id, change.ParentFrameId, change.SourceSeat, change.TargetSeat, change.Kind, change.Amount, change.HpBefore, change.HpAfter) && hp.CandidateIndex >= 0 && hp.CandidateIndex < hp.Candidates.Count)
        {
            var c = hp.Candidates[hp.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.BindingId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        if (f.TriggerId == "consume-name" && getFrameStore()[at - 1] is ProgramCardTriggerWindowFrame p && p.Id == w.ParentFrameId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && getCharacterChengYuMouRuntime().MouCyUse(w)is not null)
        {
            var c = p.Candidates[p.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        return false;
    }

    public MouCyUse? Use(ProgramSkillWindowContext c) => getCharacterChengYuMouRuntime().MouCyUse(c);
    public bool At(int id, CardLocation location) => getCardZoneStore().GetLocation(id) == location;
    public bool SameChoice(PromptChoice a, PromptChoice b) => EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([a], [b]);
    public bool DamageCard(CardKind k) => EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(k) || EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(k) || k == CardKind.Lightning;
    public SkillPromptPresentation Presentation(string id)
    {
        var s = getContentRegistry().GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }
}
