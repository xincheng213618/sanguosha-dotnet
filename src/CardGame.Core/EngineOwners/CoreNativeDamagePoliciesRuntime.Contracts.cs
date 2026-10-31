#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDamagePoliciesCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreNativeDamagePoliciesCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed);
}

internal interface ICoreNativeDamagePoliciesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTengjia(global::CardGame.Core.CharacterState player);
    bool HasSilverLion(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeDamagePoliciesCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FinalTargetSlashEscalationBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack, int damageWithoutEscalation);
    bool HasFinalTargetSlashHpDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTargetSlashReceipt> FinalTargetSlashReceipts(long frameId, int target);
}

internal interface ICoreNativeDamagePoliciesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeDamagePoliciesCoreShenwuEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature ShenwuDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature original);
}

internal interface ICoreNativeDamagePoliciesCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeDamagePoliciesModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreNativeDamagePoliciesCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeDamagePoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreNativeDamagePoliciesCharacterDingYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureAdoptedChildSlashDamageIssue(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreNativeDamagePoliciesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeDamagePoliciesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeDamagePoliciesCoreQiaoquHaoduoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? BorrowedSwordSlashDamageModifierOwner(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreShanheSchoolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId);
}

internal interface ICoreNativeDamagePoliciesCoreSlashUseOrdinalsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSlashUseOrdinalModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, global::CardGame.Core.GameEngine.IDamageAttempt? attack, int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeDamagePoliciesCoreShanheS5QuyiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheS5JiaoziQualified(int ownerSeat);
}

internal interface ICoreNativeDamagePoliciesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalGudingBlade { get; }

    bool UsesFormalTengjia { get; }

    bool UsesFormalSilverLion { get; }
}

internal interface ICoreNativeDamagePoliciesCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CapturedWholeActualTurnTrickUseOrdinal(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreDirectSkillDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectSkillDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeDamagePoliciesCorePassiveRulePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPublicEnemyParticipantDamageModifiers { get; }
}

internal interface ICoreNativeDamagePoliciesCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangFixedDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreNativeDamagePoliciesCoreShanheSchoolLateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ProgramDamageModifierAmount(global::CardGame.Core.SkillProgramDamageModifier modifier);
}

internal interface ICoreNativeDamagePoliciesCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.CardUseEffectSource Source, int Cap)? GetPassiveProgramDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreNativeDamagePoliciesCoreHeartSlashDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreShanheBadaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesShanheBadaoDamageModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, int sourceSeat, bool chain);
}

internal interface ICoreNativeDamagePoliciesCharacterCheZhouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, out int amount);
    void RecordAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int previousAmount, int replacement);
}

internal interface ICoreNativeDamagePoliciesCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature? ShouliThunderNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamagePoliciesCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeDamagePoliciesCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboRepeated(long resolutionId);
}

internal interface ICoreNativeDamagePoliciesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
