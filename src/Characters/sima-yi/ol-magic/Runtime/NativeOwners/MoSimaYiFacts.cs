#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSimaYiFacts(global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry) : IMoSimaYiFacts
{
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public bool IsGameOver => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool IsAlive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool IsHuman(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public IReadOnlyList<T> History<T>()
        where T : IGameEvent => Array.AsReadOnly(getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>().ToArray());
    public IReadOnlyList<CardConversionSource> GuiFuOwners() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(p, MoSimaYiComposition.Guifu)).Select(p => new CardConversionSource(MoSimaYiComposition.Guifu, "acquire-round", p.Seat, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(p, MoSimaYiComposition.Guifu))).ToArray());
    public IReadOnlyList<string> EffectiveSkills(int owner) => Array.AsReadOnly(getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(matchParticipantsState.ReferenceToPlayers[owner]).ActiveGrants.Select(g => g.SkillId).Distinct(StringComparer.Ordinal).ToArray());
    public IReadOnlyList<int> RetaliationActors(int owner, int round) => Array.AsReadOnly(getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MoSimaYiDamageCauseEvent>().Where(e => e.Cause.Round == round && e.Cause.SourceSeat == owner).Select(e => e.Cause.TargetSeat).Where(IsAlive).Distinct().OrderBy(seat => (seat - owner + matchParticipantsState.ReferenceToPlayers.Count) % matchParticipantsState.ReferenceToPlayers.Count).ToArray());
    public MoSimaYiDodge? FindDodge()
    {
        foreach (var location in new[]
        {
            CardLocation.DrawPile,
            CardLocation.DiscardPile
        }

        )
            if (getCardZoneStore().CardsAt(location).FirstOrDefault(c => c.Kind == CardKind.Dodge)is { } card)
                return new(card.Id, location);
        return null;
    }

    public string SkillName(string id) => getContentRegistry().GetSkill(id).Name;
    public bool HasJudgmentArea(int actor) => !matchParticipantsState.ReferenceToPlayers[actor].JudgmentAreaAbolished;
}
