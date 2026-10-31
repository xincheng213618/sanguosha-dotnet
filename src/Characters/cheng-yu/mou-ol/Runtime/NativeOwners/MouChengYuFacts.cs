#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengYuMouOlEntries> getCharacterChengYuMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries> getCoreGameEngineDesignatedExtraTargetsRuntime) : IMouChengYuFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public bool GameOver => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public IReadOnlyList<T> History<T>()
        where T : IGameEvent => Array.AsReadOnly(getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>().ToArray());
    public CardActionContext Action(ProgramSkillFrame f) => getCharacterChengYuMouOlRuntime().MouChengYuAction(f);
    public CardUseFrame? Use(long id) => getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(id);
    public CardUseFrame? TargetUse(ProgramSkillFrame f) => f.SkillId == MouChengYuComposition.Lidan && f.TriggerId == "next-matching-targets" && NativeParentMatches(f) && f.WindowContext?.CardUse is { } c ? getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(c.ParentCardUseFrameId) : null;
    public MouDanchiDamage? Damage(ProgramSkillFrame f) => getCharacterChengYuMouOlRuntime().MouChengYuDamage(f.OwnerSeat, f.WindowContext);
    public bool NativeParentMatches(ProgramSkillFrame f) => getCharacterChengYuMouOlRuntime().MouChengYuParentMatches(f);
    public IReadOnlyList<MouChengYuTargetOption> TargetOptions(long id, IReadOnlyList<int> selected)
    {
        if (getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(id)is not { Action: { } a } use || !Alive(use.SourceSeat))
            return[];
        // An entity-backed delayed placement has exactly one native Judgment
        // destination. Extra targeting must not clone its physical entity.
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind))
            return[];
        var original = use.TargetSeats.Count == 0 && use.CardKind == CardKind.DrawTwo ? new[]
        {
            use.SourceSeat
        }

        : use.TargetSeats;
        var targets = Array.AsReadOnly(original.Concat(selected).ToArray());
        var preview = use with
        {
            TargetSeats = targets,
            Action = getCoreGameEngineDesignatedExtraTargetsRuntime().CloneDesignatedExtraTargetAction(a, targets)
        };
        return Array.AsReadOnly(getCoreGameEngineDesignatedExtraTargetsRuntime().DesignatedExtraTargetCandidates(preview).Select(seats => new MouChengYuTargetOption(string.Join("、", seats.Select(s => matchParticipantsState.ReferenceToPlayers[s].Name))) { Seats = seats }).ToArray());
    }

    public IReadOnlyList<int> SlashTargets(int actor) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(t => getCharacterChengYuMouOlRuntime().CanMouDanchiSlash(actor, t.Seat)).Select(t => t.Seat).ToArray());
}
