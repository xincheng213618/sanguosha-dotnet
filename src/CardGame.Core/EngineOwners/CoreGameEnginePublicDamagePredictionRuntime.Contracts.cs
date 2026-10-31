#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicDamagePredictionCoreHeartSlashDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit PublicProposedSlashSuit(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind);
    bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICorePublicDamagePredictionCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePublicDamagePredictionCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICorePublicDamagePredictionCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICorePublicDamagePredictionCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICorePublicDamagePredictionCoreDirectSkillDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectSkillDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionCoreShanheS5QuyiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ShanheS5JiaoziPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards = 0);
}

internal interface ICorePublicDamagePredictionCoreNativeDamageArmorPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICorePublicDamagePredictionCoreWushuangPublicPredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PublicDamagePrediction AttachWushuangPublicCombatPrediction(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat, int amount, global::CardGame.Core.CardKind kind, int paidHand, global::CardGame.Core.PublicDamagePrediction legacy);
    int WushuangPublicNativeDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, global::CardGame.Core.CardKind? kind, global::CardGame.Core.Suit suit, int additionalIncomingSegments, int priorTargetSegments, long? issuedUseId = default);
}

internal interface ICorePublicDamagePredictionCoreShanheWoxinOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int WoxinPublicBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack);
}

internal interface ICorePublicDamagePredictionCoreShanheSchoolLateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ProgramDamageModifierAmount(global::CardGame.Core.SkillProgramDamageModifier modifier);
}

internal interface ICorePublicDamagePredictionCoreShanheBadaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesShanheBadaoDamageModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, int sourceSeat, bool chain);
}

internal interface ICorePublicDamagePredictionSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICorePublicDamagePredictionCharacterPanZhangMaZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryAnjianDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionCharacterQuYiClassicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ClassicJiaoziPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards);
}

internal interface ICorePublicDamagePredictionCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICorePublicDamagePredictionCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role);
}

internal interface ICorePublicDamagePredictionCoreShanheSchoolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId);
}

internal interface ICorePublicDamagePredictionCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
    bool HasSilverLion(global::CardGame.Core.CharacterState player);
}

internal interface ICorePublicDamagePredictionCharacterZhangFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICorePublicDamagePredictionCoreDuanhunOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICorePublicDamagePredictionCharacterChengPuBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryChengPuPublicActionBonus(int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat);
    int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICorePublicDamagePredictionCoreSlashUseOrdinalsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSlashUseOrdinalModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, global::CardGame.Core.GameEngine.IDamageAttempt? attack, int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICorePublicDamagePredictionCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICorePublicDamagePredictionCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalSilverLion { get; }
}

internal interface ICorePublicDamagePredictionCoreWushuangAuditCombatOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int WangongPublicProposedBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, global::CardGame.Core.CardKind? kind);
}

internal interface ICorePublicDamagePredictionCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
    (global::CardGame.Core.CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(global::CardGame.Core.CharacterState target, int? sourceSeat, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICorePublicDamagePredictionCoreShenwuNpcProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenwuEnabled(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICorePublicDamagePredictionCharacterHuangZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePublicDamagePredictionContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePublicDamagePredictionCoreShanheLeizhenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ShanheLeizhenPublicActionBonus(int actorSeat, global::CardGame.Core.CardKind? effectiveKind);
}

internal interface ICorePublicDamagePredictionCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICorePublicDamagePredictionCoreShanheSourceShennuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ShanheShennuPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack);
}

internal interface ICorePublicDamagePredictionCoreShanheS5ShuangxiongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetShanheS5ShuangxiongPublicDuelBaseDamage(int viewerSeat, global::CardGame.Core.LegalAction action, out int damage);
}

internal interface ICorePublicDamagePredictionCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}
