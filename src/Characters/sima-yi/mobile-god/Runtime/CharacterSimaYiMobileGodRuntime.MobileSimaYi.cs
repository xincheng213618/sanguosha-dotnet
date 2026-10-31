#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMobileGodRuntime
{
    internal static MobileSimaAttack ProjectMobileSimaAttack(NativeOwners.CardAttackHandle attack) => new(attack.ResolutionId, attack.TargetSeat, attack.IsChainPropagation, attack.SuccessfulDodgeResponses);
    internal static MobileSimaSword ProjectMobileSimaSword(NativeOwners.BorrowedSwordHandle sword) => new(sword.ResolutionId, sword.WeaponOwnerSeat, sword.SourceSeat, sword.SlashTargetSeat);
    string? IMobileSimaYiFacts.Faction(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IMobileSimaRenjieFacts.SkillInstalled(string skillId) => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(skillId);
    string IMobileSimaRenjieFacts.ContentHash => _contentRegistryOperations._contentRegistry.ContentHash;

    int IMobileSimaRenjieFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    MobileSimaDuel? IMobileSimaRenjieFacts.ActiveDuel => _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel ? new(duel.ResolutionId, duel.SuccessfulSlashResponses) : null;

    MobileSimaAttack? IMobileSimaRenjieFacts.ActiveAttack => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack ? ProjectMobileSimaAttack(attack) : null;

    MobileSimaSword? IMobileSimaRenjieFacts.ActiveSword => _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } sword ? ProjectMobileSimaSword(sword) : null;

    IReadOnlyList<Card> IMobileSimaRenjieFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat);
    bool IMobileSimaRenjieFacts.WenjiNullificationForbidden(NullificationWindowFrame frame, int actor) => _characterLiuQiOlOperations.IsWenjiNullificationResponseForbidden(frame, actor);
    bool IMobileSimaRenjieFacts.FuqiNullificationBanned(int actor) => _coreWushuangSkillsBAuditOperations.WushuangFuqiNullificationBanned(actor);
    bool IMobileSimaRenjieFacts.NearbyResponseProhibited(int source, int actor, CardKind kind, IReadOnlyList<int> targets) => _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(source, actor, kind, targets);
    bool IMobileSimaRenjieFacts.UseUnrespondable(long useId) => _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(useId);
    bool IMobileSimaRenjieFacts.YingboUnrespondable(long useId) => _characterLuMengMouOperations.IsYingboUnrespondable(useId);
    bool IMobileSimaRenjieFacts.WenjiResponseForbidden(long useId, int actor) => _characterLiuQiOlOperations.IsWenjiResponseForbidden2(useId, actor);
    bool IMobileSimaRenjieFacts.LegalBorrowedSwordTarget(int owner, int target) => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int IMobileSimaRenjieFacts.ExtraEffectOrdinal(long useId) => _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(useId);
    bool IMobileSimaRenjieFacts.NullificationEffectOwner(NullificationWindowFrame frame) => _coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(frame);
    int IMobileSimaRenjieFacts.MarkerSourceCount(int owner, PlayerMarkerKind marker, int source) => _coreMatchParticipantsPlayersAccess._players[owner].MarkerSourceCounts.GetValueOrDefault((marker, source));
    bool IMobileSimaRenjieFacts.InvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(parent, child);
    bool IMobileSimaRenjieFacts.InvocationObserverSuffix(long invocationFrameId) => _characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == invocationFrameId));
    MobileSimaPlayer IMobileSimaCommonFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman);
    bool IMobileSimaCommonFacts.HasSkill(int seat, string skillId) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skillId);
    IReadOnlyList<SkillGrant> IMobileSimaCommonFacts.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants;
    int IMobileSimaCommonFacts.MarkerCount(int seat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(marker);
    int IMobileSimaCommonFacts.TurnNumber => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    Winner IMobileSimaCommonFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    ContentSkillDefinition IMobileSimaCommonFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> IMobileSimaCommonFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    void IMobileSimaYiOperations.GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _coreProgramSkillChangesOperations.GrantProgramSkills(frame, skillIds);
    void IMobileSimaYiOperations.PayMarker(int owner, SkillProgramMarkerCost cost, string skillId, string bindingId) => _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[owner], cost, skillId, bindingId);
    void IMobileSimaYiOperations.ExtraTurn(ProgramSkillFrame frame, int target) => _coreSkillProgramsOperations.PendProgramExtraTurn(frame, target);
    bool IMobileSimaYiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    void IMobileSimaYiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
