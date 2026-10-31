#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDianWeiFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries> getCoreGameEngineEquipmentSlotGroupProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries> getCoreGameEngineForeignTurnContestsRuntime) : IMouDianWeiFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public int Hp(int s) => matchParticipantsState.ReferenceToPlayers[s].Hp;
    public int MaxHp(int s) => matchParticipantsState.ReferenceToPlayers[s].MaxHp;
    public int Hand(int s) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count;
    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public SkillProgram? Program(string s) => getContentRegistry().Skills.GetValueOrDefault(s)?.Program;
    public bool Parent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.ResolveMouDianWeiKuangzhan)
            return f.SkillId == MouDianWeiComposition.Kuangzhan && f.ActivationId == "draw-and-contest" && f.TriggerId is null && f.WindowContext is null && getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        var window = op == SkillProgramEffectOp.ResolveMouDianWeiKangyongStart ? SkillProgramTriggerWindow.TurnStartBeforeNormalFlow : SkillProgramTriggerWindow.AfterTurnEnded;
        if (f.SkillId != MouDianWeiComposition.Kangyong || f.TriggerId != f.ActivationId || f.TriggerId != (window == SkillProgramTriggerWindow.AfterTurnEnded ? "repay-actual-turn-end" : "recover-actual-turn-start") || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || c.Window != window || f.OwnerSeat != TurnOwner)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (window == SkillProgramTriggerWindow.AfterTurnEnded)
            return n > 0 && getFrameStore()[n - 1] is DeferredTurnEndFrame ended && ended.Id == c.ParentFrameId && ended.OwnerSeat == f.OwnerSeat && ended.TurnNumber == Turn && c.SourceSeat == f.OwnerSeat && c.TargetSeat == f.OwnerSeat && getCoreGameEngineAfterTurnEndedProgramsRuntime().IsActualAfterTurnEndedParent(ended) && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, getCoreGameEngineAfterTurnEndedProgramsRuntime().AfterTurnEndedCandidate(ended));
        return n > 0 && getFrameStore()[n - 1] is ProgramLifecycleTriggerWindowFrame p && p.Id == c.ParentFrameId && p.Window == window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]);
    }

    public IReadOnlyList<int> Opponents(int owner) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[owner]).Count == 0 && !getCharacterLuKangWumiaoRuntime().CanWuLuKangDiscardForPindian(matchParticipantsState.ReferenceToPlayers[owner]) ? [] : matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner && getCoreGameEngineEquipmentSlotGroupProgramsRuntime().CanBePindianTarget(owner, p.Seat) && (getCorePhysicalOwnedCardQueriesRuntime().GetHand(p).Count > 0 || getCharacterLuKangWumiaoRuntime().CanWuLuKangDiscardForPindian(p))).Select(p => p.Seat).Order().ToArray();
    public IReadOnlyList<int> NotWon(int owner) => getCoreGameEnginePlayCardHistoryRuntime().EventsSinceLastBoundary(x => x is TurnStartedEvent or TurnEndedEvent).OfType<PindianResultDeterminedEvent>().SelectMany(x => new[] { x.Result.SourceSeat, x.Result.OpponentSeat }.Where(s => !x.Result.WonBy(s))).Where(s => s != owner && Alive(s)).Distinct().Order().ToArray();
    public bool CanSlash(int actor, int target) => getCoreGameEngineForeignTurnContestsRuntime().CanUseForeignTurnContestSlash(actor, target);
    public PindianResult Result(ProgramSkillFrame f)
    {
        var r = f.MouDianWei ?? throw new InvalidOperationException("Missing contest parent receipt.");
        var bind = MouDianWeiRuntime.Bind(r);
        var issued = getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouDianWeiContestIssuedEvent>().Where(p => p.FrameId == f.Id && p.Index == r.Index).ToArray();
        var actual = getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<PindianResultDeterminedEvent>().Where(p => p.FrameId == r.ContestId && p.SkillId == f.SkillId).ToArray();
        if (issued is not [var start] || start.ContestId != r.ContestId || start.Opponent != r.Opponent || start.Bind != bind || actual is not [var fact] || f.PindianResultBindings.SingleOrDefault(b => b.Name == bind)is not { } b || b.SourceSeat != f.OwnerSeat || b.OpponentSeat != r.Opponent || b.SourceRank != fact.Result.SourceRank || b.OpponentRank != fact.Result.OpponentRank || b.SourceWon != fact.Result.SourceWon || b.Visibility != SkillProgramCardSetVisibility.Public || fact.Result.SourceSeat != f.OwnerSeat || fact.Result.OpponentSeat != r.Opponent)
            throw new InvalidOperationException("Kuangzhan requires its exact native Pindian result and typed binding.");
        return fact.Result;
    }
}
