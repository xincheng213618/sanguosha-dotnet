#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime))]
internal sealed class CorePublicDamagePredictionCoreHeartSlashDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreHeartSlashDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit PublicProposedSlashSuit(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind) => this._targetFactory().PublicProposedSlashSuit(view, actorSeat, action, kind);
    public bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().MatchesOwnerUsedHeartSlash(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CorePublicDamagePredictionCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CorePublicDamagePredictionCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CorePublicDamagePredictionCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CorePublicDamagePredictionCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime))]
internal sealed class CorePublicDamagePredictionCoreDirectSkillDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreDirectSkillDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> _targetFactory = targetFactory;
    public bool IsDirectSkillDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().IsDirectSkillDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheS5QuyiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheS5QuyiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _targetFactory = targetFactory;
    public int ShanheS5JiaoziPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards = 0) => this._targetFactory().ShanheS5JiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeDamageArmorPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeDamageArmorPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().HasNativeDamageArmorPolicy(sourceSeat, sourceLess, kind, visibleSkillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime))]
internal sealed class CorePublicDamagePredictionCoreWushuangPublicPredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreWushuangPublicPredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangPublicPredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PublicDamagePrediction AttachWushuangPublicCombatPrediction(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat, int amount, global::CardGame.Core.CardKind kind, int paidHand, global::CardGame.Core.PublicDamagePrediction legacy) => this._targetFactory().AttachWushuangPublicCombatPrediction(view, actorSeat, action, targetSeat, amount, kind, paidHand, legacy);
    public int WushuangPublicNativeDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, global::CardGame.Core.CardKind? kind, global::CardGame.Core.Suit suit, int additionalIncomingSegments, int priorTargetSegments, long? issuedUseId = default) => this._targetFactory().WushuangPublicNativeDamageBonus(view, sourceSeat, targetSeat, kind, suit, additionalIncomingSegments, priorTargetSegments, issuedUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheWoxinAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheWoxinOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> _targetFactory = targetFactory;
    public int WoxinPublicBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack) => this._targetFactory().WoxinPublicBonus(view, sourceSeat, sourceLess, chain, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheSchoolLateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheSchoolLateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> _targetFactory = targetFactory;
    public int ProgramDamageModifierAmount(global::CardGame.Core.SkillProgramDamageModifier modifier) => this._targetFactory().ProgramDamageModifierAmount(modifier);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheBadaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheBadaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> _targetFactory = targetFactory;
    public bool MatchesShanheBadaoDamageModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, int sourceSeat, bool chain) => this._targetFactory().MatchesShanheBadaoDamageModifier(modifier, sourceSeat, chain);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed class CorePublicDamagePredictionCharacterPanZhangMaZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCharacterPanZhangMaZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryAnjianDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryAnjianDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime))]
internal sealed class CorePublicDamagePredictionCharacterQuYiClassicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCharacterQuYiClassicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime> _targetFactory = targetFactory;
    public int ClassicJiaoziPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards) => this._targetFactory().ClassicJiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CorePublicDamagePredictionCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasWeaponAbility(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CorePublicDamagePredictionCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => this._targetFactory().HasSkillRoleQualification(owner, skillId, instance, role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheSchoolsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheSchoolsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> _targetFactory = targetFactory;
    public bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId) => this._targetFactory().IsShanheSchoolSkillQualified(ownerSeat, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasArmorBypass(global::CardGame.Core.CharacterState player) => this._targetFactory().HasArmorBypass(player);
    public bool HasSilverLion(global::CardGame.Core.CharacterState player) => this._targetFactory().HasSilverLion(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed class CorePublicDamagePredictionCharacterZhangFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCharacterZhangFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryZhangFeiLiyongCardDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime))]
internal sealed class CorePublicDamagePredictionCoreDuanhunAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreDuanhunOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> _targetFactory = targetFactory;
    public bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().DuanhunBypassesNativeDamageArmor(sourceSeat, sourceLess, visibleSkillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime))]
internal sealed class CorePublicDamagePredictionCharacterChengPuBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCharacterChengPuBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryChengPuPublicActionBonus(int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat) => this._targetFactory().BoundaryChengPuPublicActionBonus(actorSeat, action, targetSeat);
    public int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryChengPuLihuoDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CorePublicDamagePredictionCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed class CorePublicDamagePredictionCoreSlashUseOrdinalsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreSlashUseOrdinalsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _targetFactory = targetFactory;
    public bool MatchesSlashUseOrdinalModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, global::CardGame.Core.GameEngine.IDamageAttempt? attack, int actorSeat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().MatchesSlashUseOrdinalModifier(modifier, attack, actorSeat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CorePublicDamagePredictionCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalSilverLion { get => this._targetFactory().UsesFormalSilverLion; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime))]
internal sealed class CorePublicDamagePredictionCoreWushuangAuditCombatAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreWushuangAuditCombatOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> _targetFactory = targetFactory;
    public int WangongPublicProposedBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, global::CardGame.Core.CardKind? kind) => this._targetFactory().WangongPublicProposedBonus(view, sourceSeat, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime))]
internal sealed class CorePublicDamagePredictionCorePassiveDamageCapsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCorePassiveDamageCapsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _targetFactory = targetFactory;
    public int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().GetIncomingFixedDamageCap(target, visibleSkillIds);
    public (global::CardGame.Core.CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(global::CardGame.Core.CharacterState target, int? sourceSeat, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().FindPassiveProgramDamageCap(target, sourceSeat, visibleSkillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed class CorePublicDamagePredictionCoreShenwuNpcProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShenwuNpcProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> _targetFactory = targetFactory;
    public bool ShenwuEnabled(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().ShenwuEnabled(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime))]
internal sealed class CorePublicDamagePredictionCharacterHuangZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCharacterHuangZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryHuangZhongCardDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheLeizhenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheLeizhenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> _targetFactory = targetFactory;
    public int ShanheLeizhenPublicActionBonus(int actorSeat, global::CardGame.Core.CardKind? effectiveKind) => this._targetFactory().ShanheLeizhenPublicActionBonus(actorSeat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CorePublicDamagePredictionCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheSourceShennuAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheSourceShennuOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime> _targetFactory = targetFactory;
    public int ShanheShennuPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack) => this._targetFactory().ShanheShennuPublicDamageBonus(view, sourceSeat, sourceLess, chain, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime))]
internal sealed class CorePublicDamagePredictionCoreShanheS5ShuangxiongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreShanheS5ShuangxiongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> _targetFactory = targetFactory;
    public bool TryGetShanheS5ShuangxiongPublicDuelBaseDamage(int viewerSeat, global::CardGame.Core.LegalAction action, out int damage) => this._targetFactory().TryGetShanheS5ShuangxiongPublicDuelBaseDamage(viewerSeat, action, out damage);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CorePublicDamagePredictionCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}
