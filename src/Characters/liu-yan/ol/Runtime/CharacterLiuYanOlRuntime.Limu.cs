#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    internal bool LiuYanLimuMaterialLegal(CharacterState owner, Card card, string skill, string? instance = null)
    {
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        return owner.IsAlive && from.OwnerSeat == owner.Seat && from.Zone is (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain) && (from.Zone != CardZoneKind.WoodenOxGrain || _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(owner).Any(c => c.Id == card.Id)) && !card.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner.Seat, skill, instance ?? _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, skill), card) && _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) == Suit.Diamond && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(owner, card) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, [card.Id]) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(owner.Seat, Suit.Diamond) && !owner.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(owner, CardKind.Indulgence) && !_coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Indulgence, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Indulgence) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(owner, CardKind.Indulgence, [owner.Seat]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Indulgence, Suit.Diamond, true) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, Suit.Diamond);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27231, SkillProgramEffectOp.UseLimuSelfIndulgenceAndRecover)]
    internal bool CanActivateLiuYanLimu(CharacterState owner, string skillId) => _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Any(c => LiuYanLimuMaterialLegal(owner, c, skillId));
    internal bool HasLiuYanLimuTargetAllowance(CharacterState actor, CharacterState target, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null) => HasLiuYanCapability && actor.IsAlive && target.IsAlive && actor.Seat != target.Seat && _coreNationalSkillsOperations.HasRuntimeSkill(actor, "ol:limu") && _corePhysicalOwnedCardQueriesOperations.GetJudgment(actor).Count > 0 && (_coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(actor.Seat, target.Seat) || global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, target, excludedEquipmentId, excludedEquipmentSlot)) <= global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(actor, excludedEquipmentId, excludedEquipmentSlot)));
    internal bool HasLiuYanLimuQuotaAllowance(CharacterState actor, CharacterState target, Card? physical, IReadOnlyList<int>? ids, int? excludedId, EquipmentSlot? excludedSlot)
    {
        if (!HasLiuYanCapability)
            return false;
        var materials = (ids ?? (physical is { Id: > 0 } ? [physical.Id] : [])).ToHashSet();
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(actor).Where(c => materials.Contains(c.Id)).ToArray();
        excludedId ??= equipment.FirstOrDefault(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.Weapon)?.Id;
        if (excludedSlot is null && equipment.Any(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.OffensiveHorse))
            excludedSlot = EquipmentSlot.OffensiveHorse;
        return HasLiuYanLimuTargetAllowance(actor, target, excludedId, excludedSlot);
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 80)]
    internal bool DistanceCardUseExemption80HasLiuYanLimuTargetAllowance(ProgramCardDistanceQuery q) => !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && HasLiuYanLimuTargetAllowance(q.Actor, q.Target, q.ExcludedEquipmentId, q.ExcludedEquipmentSlot);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 80)]
    internal bool LiuYanLimuSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => HasLiuYanLimuQuotaAllowance(query.Actor, query.Target, query.PhysicalCard, query.PhysicalCardIds, query.ExcludedEquipmentId, query.ExcludedEquipmentSlot);
}
