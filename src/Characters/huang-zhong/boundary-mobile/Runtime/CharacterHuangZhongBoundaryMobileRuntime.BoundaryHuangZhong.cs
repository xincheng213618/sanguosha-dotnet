#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZhongBoundaryMobileRuntime
{
    internal const string BoundaryHuangZhongMobileSkill = "boundary:liegong-mobile";
    internal bool IsBoundaryHuangZhongOrdinaryRange(CharacterState actor, CharacterState target, CardKind kind, int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Any(i => i.SkillId == BoundaryHuangZhongMobileSkill) && (excludedEquipmentId is null && excludedEquipmentSlot is null ? _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actor.Seat, target.Seat) : _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(actor.Seat, target.Seat) || global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, target, excludedEquipmentId, excludedEquipmentSlot)) <= global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(actor, excludedEquipmentId, excludedEquipmentSlot)));
    internal bool IsBoundaryHuangZhongUnrespondable(long frameId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == frameId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(frameId, attack.TargetSeat).Any(r => r.SkillId == BoundaryHuangZhongMobileSkill && r.PreventCancellation);
    internal int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is global::CardGame.Core.NativeOwners.CardAttackHandle && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) ? _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Where(r => r.SkillId == BoundaryHuangZhongMobileSkill).Sum(r => r.DamageBonus) : 0;
}
