#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMobileGodRuntime
{
    internal static MobileSimaAttack ProjectMobileSimaAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => new(attack.ResolutionId, attack.TargetSeat, attack.IsChainPropagation, attack.SuccessfulDodgeResponses);
    internal static MobileSimaSword ProjectMobileSimaSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle sword) => new(sword.ResolutionId, sword.WeaponOwnerSeat, sword.SourceSeat, sword.SlashTargetSeat);
    string? global::CardGame.Core.IMobileSimaYiFacts.Faction(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.SkillInstalled(string skillId) => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(skillId);
    string global::CardGame.Core.IMobileSimaRenjieFacts.ContentHash => _contentRegistryOperations._contentRegistry.ContentHash;

    int global::CardGame.Core.IMobileSimaRenjieFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    MobileSimaDuel? global::CardGame.Core.IMobileSimaRenjieFacts.ActiveDuel => _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel ? new(duel.ResolutionId, duel.SuccessfulSlashResponses) : null;

    MobileSimaAttack? global::CardGame.Core.IMobileSimaRenjieFacts.ActiveAttack => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack ? ProjectMobileSimaAttack(attack) : null;

    MobileSimaSword? global::CardGame.Core.IMobileSimaRenjieFacts.ActiveSword => _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } sword ? ProjectMobileSimaSword(sword) : null;

    IReadOnlyList<Card> global::CardGame.Core.IMobileSimaRenjieFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(seat);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.WenjiNullificationForbidden(NullificationWindowFrame frame, int actor) => _characterLiuQiOlOperations.IsWenjiNullificationResponseForbidden(frame, actor);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.FuqiNullificationBanned(int actor) => _coreWushuangSkillsBAuditOperations.WushuangFuqiNullificationBanned(actor);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.NearbyResponseProhibited(int source, int actor, CardKind kind, IReadOnlyList<int> targets) => _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(source, actor, kind, targets);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.UseUnrespondable(long useId) => _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(useId);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.YingboUnrespondable(long useId) => _characterLuMengMouOperations.IsYingboUnrespondable(useId);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.WenjiResponseForbidden(long useId, int actor) => _characterLiuQiOlOperations.IsWenjiResponseForbidden(useId, actor);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.LegalBorrowedSwordTarget(int owner, int target) => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int global::CardGame.Core.IMobileSimaRenjieFacts.ExtraEffectOrdinal(long useId) => _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(useId);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.NullificationEffectOwner(NullificationWindowFrame frame) => _coreDelayedNullificationOperations.HasNullificationEffectOwner(frame);
    int global::CardGame.Core.IMobileSimaRenjieFacts.MarkerSourceCount(int owner, PlayerMarkerKind marker, int source) => _coreMatchParticipantsPlayersAccess._players[owner].MarkerSourceCounts.GetValueOrDefault((marker, source));
    bool global::CardGame.Core.IMobileSimaRenjieFacts.InvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(parent, child);
    bool global::CardGame.Core.IMobileSimaRenjieFacts.InvocationObserverSuffix(long invocationFrameId) => _characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == invocationFrameId));
    MobileSimaPlayer global::CardGame.Core.IMobileSimaCommonFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman);
    bool global::CardGame.Core.IMobileSimaCommonFacts.HasSkill(int seat, string skillId) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skillId);
    IReadOnlyList<SkillGrant> global::CardGame.Core.IMobileSimaCommonFacts.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants;
    int global::CardGame.Core.IMobileSimaCommonFacts.MarkerCount(int seat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(marker);
    int global::CardGame.Core.IMobileSimaCommonFacts.TurnNumber => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    Winner global::CardGame.Core.IMobileSimaCommonFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    ContentSkillDefinition global::CardGame.Core.IMobileSimaCommonFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> global::CardGame.Core.IMobileSimaCommonFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    void global::CardGame.Core.IMobileSimaYiOperations.GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _coreProgramSkillChangesOperations.GrantProgramSkills(frame, skillIds);
    void global::CardGame.Core.IMobileSimaYiOperations.PayMarker(int owner, SkillProgramMarkerCost cost, string skillId, string bindingId) => _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[owner], cost, skillId, bindingId);
    void global::CardGame.Core.IMobileSimaYiOperations.ExtraTurn(ProgramSkillFrame frame, int target) => _coreSkillProgramsOperations.PendProgramExtraTurn(frame, target);
    bool global::CardGame.Core.IMobileSimaYiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    void global::CardGame.Core.IMobileSimaYiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
