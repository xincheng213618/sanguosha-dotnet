#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ProgramConversionDamageBonus(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreProgramDamageHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed) => this._targetFactory().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasTengjia(global::CardGame.Core.CharacterState player) => this._targetFactory().HasTengjia(player);
    public bool HasSilverLion(global::CardGame.Core.CharacterState player) => this._targetFactory().HasSilverLion(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public int FinalTargetSlashEscalationBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack, int damageWithoutEscalation) => this._targetFactory().FinalTargetSlashEscalationBonus(attack, damageWithoutEscalation);
    public bool HasFinalTargetSlashHpDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasFinalTargetSlashHpDamage(attack);
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTargetSlashReceipt> FinalTargetSlashReceipts(long frameId, int target) => this._targetFactory().FinalTargetSlashReceipts(frameId, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreShenwuEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreShenwuEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature ShenwuDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature original) => this._targetFactory().ShenwuDamageNature(attack, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreTeamActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreTeamActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target) => this._targetFactory().PublicTeamEnemy(owner, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime))]
internal sealed class CoreNativeDamagePoliciesCharacterDingYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCharacterDingYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime> _targetFactory = targetFactory;
    public void CaptureAdoptedChildSlashDamageIssue(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CaptureAdoptedChildSlashDamageIssue(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeDamagePoliciesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreQiaoquHaoduoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreQiaoquHaoduoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> _targetFactory = targetFactory;
    public int? BorrowedSwordSlashDamageModifierOwner(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BorrowedSwordSlashDamageModifierOwner(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreShanheSchoolsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreShanheSchoolsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> _targetFactory = targetFactory;
    public bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId) => this._targetFactory().IsShanheSchoolSkillQualified(ownerSeat, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreSlashUseOrdinalsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreSlashUseOrdinalsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _targetFactory = targetFactory;
    public bool MatchesSlashUseOrdinalModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, global::CardGame.Core.GameEngine.IDamageAttempt? attack, int actorSeat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().MatchesSlashUseOrdinalModifier(modifier, attack, actorSeat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreShanheS5QuyiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreShanheS5QuyiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _targetFactory = targetFactory;
    public bool IsShanheS5JiaoziQualified(int ownerSeat) => this._targetFactory().IsShanheS5JiaoziQualified(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalGudingBlade { get => this._targetFactory().UsesFormalGudingBlade; }
    public bool UsesFormalTengjia { get => this._targetFactory().UsesFormalTengjia; }
    public bool UsesFormalSilverLion { get => this._targetFactory().UsesFormalSilverLion; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreWholeActualTurnUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreWholeActualTurnUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _targetFactory = targetFactory;
    public int? CapturedWholeActualTurnTrickUseOrdinal(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CapturedWholeActualTurnTrickUseOrdinal(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreDirectSkillDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreDirectSkillDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> _targetFactory = targetFactory;
    public bool IsDirectSkillDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().IsDirectSkillDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime))]
internal sealed class CoreNativeDamagePoliciesCorePassiveRulePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCorePassiveRulePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasPublicEnemyParticipantDamageModifiers { get => this._targetFactory().HasPublicEnemyParticipantDamageModifiers; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreWushuangCombatTacticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreWushuangCombatTacticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _targetFactory = targetFactory;
    public bool WushuangFixedDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().WushuangFixedDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreShanheSchoolLateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreShanheSchoolLateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolLateRuntime> _targetFactory = targetFactory;
    public int ProgramDamageModifierAmount(global::CardGame.Core.SkillProgramDamageModifier modifier) => this._targetFactory().ProgramDamageModifierAmount(modifier);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime))]
internal sealed class CoreNativeDamagePoliciesCorePassiveDamageCapsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCorePassiveDamageCapsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.CardUseEffectSource Source, int Cap)? GetPassiveProgramDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetPassiveProgramDamageCap(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreHeartSlashDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreHeartSlashDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> _targetFactory = targetFactory;
    public bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().MatchesOwnerUsedHeartSlash(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreShanheBadaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreShanheBadaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> _targetFactory = targetFactory;
    public bool MatchesShanheBadaoDamageModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, int sourceSeat, bool chain) => this._targetFactory().MatchesShanheBadaoDamageModifier(modifier, sourceSeat, chain);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed class CoreNativeDamagePoliciesCharacterCheZhouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCharacterCheZhouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryGetAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, out int amount) => this._targetFactory().TryGetAnniversaryCheZhouDamageReplacement(attack, out amount);
    public void RecordAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int previousAmount, int replacement) => this._targetFactory().RecordAnniversaryCheZhouDamageReplacement(attack, previousAmount, replacement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreNativeDamagePoliciesCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature? ShouliThunderNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ShouliThunderNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasWeaponAbility(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeDamagePoliciesCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamagePoliciesCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public bool IsYingboRepeated(long resolutionId) => this._targetFactory().IsYingboRepeated(resolutionId);
}
