#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSimaFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, Func<RuntimeEntries.CoreActiveNativeAttackQueriesEntries> getCoreActiveNativeAttackQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CharacterLiuQiOlEntries> getCharacterLiuQiOlRuntime, Func<RuntimeEntries.CoreGameEngineWushuangSkillsBAuditEntries> getCoreGameEngineWushuangSkillsBAuditRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries> getCoreGameEngineIssuedPlayUsePoliciesRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreNativeBorrowedSwordEntries> getCoreNativeBorrowedSwordRuntime, Func<RuntimeEntries.CharacterFeiYiOlEntries> getCharacterFeiYiOlRuntime, Func<RuntimeEntries.CoreGameEngineDelayedNullificationEntries> getCoreGameEngineDelayedNullificationRuntime, Func<RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime, Func<FrameStore> getFrameStore) : IMobileSimaYiFacts, IMobileSimaRenjieFacts
{
    public MobileSimaPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman);
    public bool HasSkill(int seat, string skillId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[seat], skillId);
    public IReadOnlyList<SkillGrant> Grants(int seat) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grants;
    public int MarkerCount(int seat, PlayerMarkerKind marker) => matchParticipantsState.ReferenceToPlayers[seat].Markers.GetValueOrDefault(marker);
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public string? Faction(int seat) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool SkillInstalled(string skillId) => getContentRegistry().Skills.ContainsKey(skillId);
    public string ContentHash => getContentRegistry().ContentHash;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public MobileSimaDuel? ActiveDuel => getCoreNativeCardContinuationCursorsRuntime().ActiveDuel is { } duel ? new(duel.ResolutionId, duel.SuccessfulSlashResponses) : null;
    public MobileSimaAttack? ActiveAttack => getCoreActiveNativeAttackQueriesRuntime().ActiveCardAttack is { } attack ? EngineOwners.CharacterSimaYiMobileGodRuntime.ProjectMobileSimaAttack(attack) : null;
    public MobileSimaSword? ActiveSword => getCoreNativeCardContinuationCursorsRuntime().ActiveBorrowedSword is { } sword ? EngineOwners.CharacterSimaYiMobileGodRuntime.ProjectMobileSimaSword(sword) : null;

    public IReadOnlyList<Card> Equipment(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat);
    public bool WenjiNullificationForbidden(NullificationWindowFrame frame, int actor) => getCharacterLiuQiOlRuntime().IsWenjiNullificationResponseForbidden(frame, actor);
    public bool FuqiNullificationBanned(int actor) => getCoreGameEngineWushuangSkillsBAuditRuntime().WushuangFuqiNullificationBanned(actor);
    public bool NearbyResponseProhibited(int source, int actor, CardKind kind, IReadOnlyList<int> targets) => getCoreGameEngineProgramCardPoliciesRuntime().IsNearbyTargetResponseProhibited(source, actor, kind, targets);
    public bool UseUnrespondable(long useId) => getCoreGameEngineIssuedPlayUsePoliciesRuntime().IsIssuedCardUnrespondable(useId);
    public bool YingboUnrespondable(long useId) => getCharacterLuMengMouRuntime().IsYingboUnrespondable(useId);
    public bool WenjiResponseForbidden(long useId, int actor) => getCharacterLiuQiOlRuntime().IsWenjiResponseForbidden2(useId, actor);
    public bool LegalBorrowedSwordTarget(int owner, int target) => getCoreNativeBorrowedSwordRuntime().IsLegalBorrowedSwordSlashTarget(matchParticipantsState.ReferenceToPlayers[owner], matchParticipantsState.ReferenceToPlayers[target]);
    public int ExtraEffectOrdinal(long useId) => getCharacterFeiYiOlRuntime().NativeExtraEffectRoundOrdinal(useId);
    public bool NullificationEffectOwner(NullificationWindowFrame frame) => getCoreGameEngineDelayedNullificationRuntime().HasNullificationEffectOwner(frame);
    public int MarkerSourceCount(int owner, PlayerMarkerKind marker, int source) => matchParticipantsState.ReferenceToPlayers[owner].MarkerSourceCounts.GetValueOrDefault((marker, source));
    public bool InvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => getCharacterHuaTuoAnniversaryGodRuntime().SkillInvocationStructuralEdge(parent, child);
    public bool InvocationObserverSuffix(long invocationFrameId) => getCharacterHuaTuoAnniversaryGodRuntime().HasSkillInvocationObserverSuffix(getFrameStore().FindIndex(frame => frame.Id == invocationFrameId));
}
