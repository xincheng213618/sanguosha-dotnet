#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCardUseRolesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreProgramCardUseRolesCharacterQuHuangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQuHuangCapability { get; }
}

internal interface ICoreProgramCardUseRolesCharacterRuanYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuanYuCapability { get; }
}

internal interface ICoreProgramCardUseRolesCoreComparedBlackSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasComparedBlackSlashPolicies { get; }
}

internal interface ICoreProgramCardUseRolesCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBlackTrickTargetPolicy { get; }
}

internal interface ICoreProgramCardUseRolesCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreProgramCardUseRolesCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdjacentRedTargetCapability { get; }
}

internal interface ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreProgramCardUseRolesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreProgramCardUseRolesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramCardUseRolesCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RefreshOwnPublicPileAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardUseFrame previous);
}

internal interface ICoreProgramCardUseRolesCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingLiubing { get; }

    bool HasWuJingHeji { get; }
}

internal interface ICoreProgramCardUseRolesCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFirstPlayUseDistance(long? frameId);
}

internal interface ICoreProgramCardUseRolesCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}

internal interface ICoreProgramCardUseRolesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCard(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramCardUseRolesCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuaManCapability { get; }

    void CaptureHuaManCompletedUse(long id, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreProgramCardUseRolesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramCardUseRolesCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreProgramCardUseRolesCoreDamageTargetBudgetOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksDamageTargetBudgetTargets { get; }
}

internal interface ICoreProgramCardUseRolesCharacterSunLuBanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AllowsBoundarySunLuBanDrawTwoRoleTarget(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardUseRolesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramCardUseRolesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreProgramCardUseRolesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreProgramCardUseRolesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreProgramCardUseRolesCoreProgramCompoundCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome SelectProgramAddedBorrowedSwordVictim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use, int weaponOwner);
}

internal interface ICoreProgramCardUseRolesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreProgramCardUseRolesCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedProvenanceUseDistance(long? useId, int owner);
    void ClearProvenanceUseOnActorChange(long useId, int actor);
}

internal interface ICoreProgramCardUseRolesCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    bool HasYuanchouCardPolicy { get; }
}

internal interface ICoreProgramCardUseRolesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramCardUseRolesCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureReplacedFactionActor(global::CardGame.Core.CardActionContext action, int actorSeat);
}

internal interface ICoreProgramCardUseRolesCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuPanCapability { get; }
}

internal interface ICoreProgramCardUseRolesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}
