#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterCaoPiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterCaoPiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> _targetFactory = targetFactory;
    public bool MouCaoPiCannotRespondTo(int responder, int source) => this._targetFactory().MouCaoPiCannotRespondTo(responder, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterXiahouHuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterXiahouHuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool AnniversaryXiahouHuiCannotRespond(int sourceSeat, int responderSeat) => this._targetFactory().AnniversaryXiahouHuiCannotRespond(sourceSeat, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterHanSongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterHanSongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHanSongUniqueHand(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasAnniversaryHanSongUniqueHand(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime))]
internal sealed class CoreProgramCardPoliciesCoreShanheTrickResponseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreShanheTrickResponseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> _targetFactory = targetFactory;
    public bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().IsShanheTrickResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime))]
internal sealed class CoreProgramCardPoliciesCoreShanheBossProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreShanheBossProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime> _targetFactory = targetFactory;
    public int GetBattleSlashCountResponseRequirement(global::CardGame.Core.CharacterState owner, int sourceSeat, global::CardGame.Core.CardKind incomingKind, global::CardGame.Core.CardKind requiredKind) => this._targetFactory().GetBattleSlashCountResponseRequirement(owner, sourceSeat, incomingKind, requiredKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreProgramCardPoliciesCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterLuZhiMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterLuZhiMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> _targetFactory = targetFactory;
    public bool MouLzCannotRespond(int source, int responder, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().MouLzCannotRespond(source, responder, kind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CoreProgramCardPoliciesCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => this._targetFactory().HasSkillRoleQualification(owner, skillId, instance, role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreProgramCardPoliciesCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool ShenZfCannotRespond(int source, int responder, global::CardGame.Core.CardKind incoming, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().ShenZfCannotRespond(source, responder, incoming, targets);
    public global::CardGame.Core.Suit? ShenZfIdentitySuit(global::CardGame.Core.CharacterState p, global::CardGame.Core.Card c, global::CardGame.Core.CardLocation? identityLocation = default) => this._targetFactory().ShenZfIdentitySuit(p, c, identityLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool HasSayingRoundRange(int actor, int target) => this._targetFactory().HasSayingRoundRange(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterXiahouBaMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterXiahouBaMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiXbPoxiCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().ShiXbPoxiCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime))]
internal sealed class CoreProgramCardPoliciesCoreLeastHandMarkerDrawProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreLeastHandMarkerDrawProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> _targetFactory = targetFactory;
    public bool IsBeforeActualDamageDistancePolicyQualified(global::CardGame.Core.CharacterState owner) => this._targetFactory().IsBeforeActualDamageDistancePolicyQualified(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterZhongHuiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterZhongHuiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiZhHuiKeChangCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().ShiZhHuiKeChangCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterXiaoQiaoBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterXiaoQiaoBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? BoundaryXiaoQiaoEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation) => this._targetFactory().BoundaryXiaoQiaoEffectiveSuit(owner, card, identityLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed class CoreProgramCardPoliciesCoreShanheS5QuyiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreShanheS5QuyiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _targetFactory = targetFactory;
    public bool IsShanheS5FuqiResponseProhibited(int sourceSeat, int responderSeat) => this._targetFactory().IsShanheS5FuqiResponseProhibited(sourceSeat, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterSunCeMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterSunCeMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime> _targetFactory = targetFactory;
    public bool ShenSunCeCannotRespond(int source, int responder, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().ShenSunCeCannotRespond(source, responder, kind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime))]
internal sealed class CoreProgramCardPoliciesCoreProgramAlcoholQuotaAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreProgramAlcoholQuotaOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> _targetFactory = targetFactory;
    public bool HasAdditionalAlcoholUse(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasAdditionalAlcoholUse(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime))]
internal sealed class CoreProgramCardPoliciesCoreWushuangSkillsBAuditAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreWushuangSkillsBAuditOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> _targetFactory = targetFactory;
    public bool WushuangFuqiCannotRespond(int source, int responder) => this._targetFactory().WushuangFuqiCannotRespond(source, responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramCardPoliciesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterLvYiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterLvYiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat) => this._targetFactory().ShiLyShishiCannotRespond(sourceSeat, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramCardPoliciesCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasBeifenPreUsePrivilege(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreProgramCardPoliciesCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().EvaluateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
    public global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().EvaluateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasMouZhaJiangPreUsePrivilege(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime))]
internal sealed class CoreProgramCardPoliciesCharacterSunShaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPoliciesCharacterSunShaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind? incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().ShiSsGanjueCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
}
