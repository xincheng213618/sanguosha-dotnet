#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoZhangFeiFacts(Func<ContentRegistry> getContentRegistry, EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<FrameStore> getFrameStore, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<CardZoneStore> getCardZoneStore) : IMoZhangFeiFacts
{
    public bool Enabled => getContentRegistry().ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoZhangFeiZhuohun);
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int ActualTurn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int MaximumHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public IReadOnlyList<string> ZhuohunGrantIds(int seat) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grants.Where(g => g.SkillId == MoZhangFeiRuntime.Zhuohun && !g.IsModeRule).Select(g => g.GrantId).ToArray());
    public bool AnyDying => getFrameStore().OfType<DyingFrame>().Any();
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public IReadOnlyList<ProgramTriggerCandidate> Candidates(int seat, SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[seat], window);
    public SkillProgramTrigger Trigger(ProgramTriggerCandidate c) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(c);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public CardKind? DamageKind(long windowId) => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger?.Id == windowId ? getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt?.EffectiveCardKind ?? getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt?.Card?.Kind : null;
    public IReadOnlyList<Card> DrawPileSlashes(int maximum) => Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DrawPile).Where(c => c.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash).Take(maximum).ToArray());
}
