#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreJudgmentCursorRuntimeRuntime>> getCoreJudgmentCursorRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramMovementSourceCountsEntries> getCoreGameEngineProgramMovementSourceCountsRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime, Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<ContentRegistry> getContentRegistry, EngineStates.CoreGameEnginePublicPileSourcesState gameEnginePublicPileSourcesState, Func<RuntimeEntries.CoreGameEngineRicePileEntries> getCoreGameEngineRicePileRuntime) : ICardsMovedWindowFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int PlayerCount => matchParticipantsState.ReferenceToPlayers.Count;

    public bool IsValidPlayerSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool HasActiveJudgment => getCoreJudgmentCursorRuntimeRuntime().Value.ActiveJudgment is not null;

    public SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(candidate);
    public SkillProgramTriggerFacts CaptureProgramTriggerFacts(int ownerSeat) => getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
    public IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(int ownerSeat, SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[ownerSeat], window);
    public IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window) => getCoreGameEngineProgramMovementSourceCountsRuntime().ProgramMovementSourceCounts(batch, window);
    public bool ContainsMovement(CardMovementRecord movement) => cardMovementJournalState.ReferenceToCardMovements.Contains(movement);
    public CardLocation? GetProgramDiscardSource(CardMovementRecord movement) => getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(movement);
    public CardKind GetAttackCardKind(int cardId) => getCoreGameEngineCardAttackStateRuntime().GetAttackCard(cardId).Kind;
    public string PlayerName(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public string SkillName(string skillId) => getContentRegistry().GetSkill(skillId).Name;
    public PublicPersistentPileSource? RicePile(ProgramTriggerCandidate candidate) => gameEnginePublicPileSourcesState.ReferenceToPublicPersistentPiles.GetValueOrDefault((candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId));
    public bool RiceProducer(PublicPersistentPileSource pile) => getCoreGameEngineRicePileRuntime().RiceProducer(pile);
}
