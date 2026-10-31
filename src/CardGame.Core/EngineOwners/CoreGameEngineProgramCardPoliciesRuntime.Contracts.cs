#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCardPoliciesCharacterCaoPiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouCaoPiCannotRespondTo(int responder, int source);
}

internal interface ICoreProgramCardPoliciesCharacterXiahouHuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AnniversaryXiahouHuiCannotRespond(int sourceSeat, int responderSeat);
}

internal interface ICoreProgramCardPoliciesCharacterHanSongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHanSongUniqueHand(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramCardPoliciesCoreShanheTrickResponseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreProgramCardPoliciesCoreShanheBossProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetBattleSlashCountResponseRequirement(global::CardGame.Core.CharacterState owner, int sourceSeat, global::CardGame.Core.CardKind incomingKind, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreProgramCardPoliciesCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreProgramCardPoliciesCharacterLuZhiMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouLzCannotRespond(int source, int responder, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreProgramCardPoliciesCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role);
}

internal interface ICoreProgramCardPoliciesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramCardPoliciesCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenZfCannotRespond(int source, int responder, global::CardGame.Core.CardKind incoming, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.Suit? ShenZfIdentitySuit(global::CardGame.Core.CharacterState p, global::CardGame.Core.Card c, global::CardGame.Core.CardLocation? identityLocation = default);
}

internal interface ICoreProgramCardPoliciesCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSayingRoundRange(int actor, int target);
}

internal interface ICoreProgramCardPoliciesCharacterXiahouBaMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiXbPoxiCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreProgramCardPoliciesCoreLeastHandMarkerDrawProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBeforeActualDamageDistancePolicyQualified(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramCardPoliciesCharacterZhongHuiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiZhHuiKeChangCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreProgramCardPoliciesCharacterXiaoQiaoBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? BoundaryXiaoQiaoEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation);
}

internal interface ICoreProgramCardPoliciesCoreShanheS5QuyiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheS5FuqiResponseProhibited(int sourceSeat, int responderSeat);
}

internal interface ICoreProgramCardPoliciesCharacterSunCeMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenSunCeCannotRespond(int source, int responder, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreProgramCardPoliciesCoreProgramAlcoholQuotaOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdditionalAlcoholUse(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramCardPoliciesCoreWushuangSkillsBAuditOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangFuqiCannotRespond(int source, int responder);
}

internal interface ICoreProgramCardPoliciesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramCardPoliciesCharacterLvYiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat);
}

internal interface ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramCardPoliciesCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramCardPoliciesCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreProgramCardPoliciesCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramCardPoliciesCharacterSunShaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind? incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}
