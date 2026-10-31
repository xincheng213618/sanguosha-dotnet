#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramCardUseRolesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterQuHuangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterQuHuangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> _targetFactory = targetFactory;
    public bool HasQuHuangCapability { get => this._targetFactory().HasQuHuangCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterRuanYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterRuanYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _targetFactory = targetFactory;
    public bool HasRuanYuCapability { get => this._targetFactory().HasRuanYuCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreComparedBlackSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreComparedBlackSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasComparedBlackSlashPolicies { get => this._targetFactory().HasComparedBlackSlashPolicies; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasBlackTrickTargetPolicy { get => this._targetFactory().HasBlackTrickTargetPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreProgramCardUseRolesCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CanUseSlashTarget(source, target, slashCard, conversionSource, effectiveKind, ignoreDistance, existingUseFrameId, noEffectiveRank, specificEffectiveRank, physicalCardIds, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreProgramCardUseRolesCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool HasAdjacentRedTargetCapability { get => this._targetFactory().HasAdjacentRedTargetCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreProgramCardUseRolesCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreProgramCardUseRolesCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public void RefreshOwnPublicPileAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardUseFrame previous) => this._targetFactory().RefreshOwnPublicPileAddedTargetCandidates(use, previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool HasWuJingLiubing { get => this._targetFactory().HasWuJingLiubing; }
    public bool HasWuJingHeji { get => this._targetFactory().HasWuJingHeji; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedFirstPlayUseDistance(long? frameId) => this._targetFactory().HasIssuedFirstPlayUseDistance(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreProgramCardUseRolesCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public bool HasSoleTargetProtectionCapability { get => this._targetFactory().HasSoleTargetProtectionCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramCardUseRolesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public bool HasTargetCard(global::CardGame.Core.CharacterState player) => this._targetFactory().HasTargetCard(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool HasHuaManCapability { get => this._targetFactory().HasHuaManCapability; }

    public void CaptureHuaManCompletedUse(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureHuaManCompletedUse(id, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreProgramCardUseRolesCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreProgramCardUseRolesCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool TracksDamageTargetBudgetTargets { get => this._targetFactory().TracksDamageTargetBudgetTargets; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterSunLuBanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterSunLuBanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool AllowsBoundarySunLuBanDrawTwoRoleTarget(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AllowsBoundarySunLuBanDrawTwoRoleTarget(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreProgramCardUseRolesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime))]
internal sealed class CoreProgramCardUseRolesCoreProgramCompoundCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreProgramCompoundCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome SelectProgramAddedBorrowedSwordVictim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use, int weaponOwner) => this._targetFactory().SelectProgramAddedBorrowedSwordVictim(frame, use, weaponOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreProvenanceCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreProvenanceCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedProvenanceUseDistance(long? useId, int owner) => this._targetFactory().HasIssuedProvenanceUseDistance(useId, owner);
    public void ClearProvenanceUseOnActorChange(long useId, int actor) => this._targetFactory().ClearProvenanceUseOnActorChange(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreProgramCardUseRolesCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    public bool HasYuanchouCardPolicy { get => this._targetFactory().HasYuanchouCardPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CoreProgramCardUseRolesCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureReplacedFactionActor(global::CardGame.Core.CardActionContext action, int actorSeat) => this._targetFactory().CaptureReplacedFactionActor(action, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreProgramCardUseRolesCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasLiuPanCapability { get => this._targetFactory().HasLiuPanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreProgramCardUseRolesCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardUseRolesCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}
