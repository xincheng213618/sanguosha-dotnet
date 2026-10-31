#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoZhangFeiFacts(global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore) : IMoZhangFeiFacts
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
