#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeEquipmentAndFactionQueriesRuntime
{
    internal bool HasBagua(CharacterState player) => _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(player, CardKind.Dodge, []) && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && (_characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => card.Kind == CardKind.BaguaFormation) || _coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Armor) == CardKind.BaguaFormation || _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.VirtualEquipment, CardKind.BaguaFormation) && !_corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Any(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor));
    internal bool HasConditionalVirtualBaguaForAi(CharacterState player) => HasBagua(player) && _coreProgramCardPoliciesOperations.CardPolicies(player, SkillProgramCardPolicyKind.VirtualEquipment, CardKind.BaguaFormation).Any(item => item.Policy.CardKinds.Contains(CardKind.BaguaFormation) && item.Policy.Condition.ContainsEquipmentPresence());
    internal bool HasTengjia(CharacterState player) => !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && (_characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => card.Kind == CardKind.Tengjia) || _coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Armor) == CardKind.Tengjia || _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.VirtualEquipment, CardKind.Tengjia) && !_corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Any(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor));
    internal bool HasSilverLion(CharacterState player) => !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && (_characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => card.Kind == CardKind.SilverLion) || _coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Armor) == CardKind.SilverLion);
    internal bool CanRequestFactionDefense(CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => !attack.FactionDefenseAttempted && owner.IsAlive && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Dodge)is not null;
    internal int GetRequiredResponseCount(CharacterState skillOwner, int sourceSeat, int responderSeat, CardKind incomingCard, CardKind requiredCardKind)
    {
        return Math.Max(_coreProgramCardPoliciesOperations.GetProgramRequiredResponseCount(skillOwner, sourceSeat, responderSeat, incomingCard, requiredCardKind), _characterLuBuHappyGodOperations.HappyLuBuRequiredResponseCount(skillOwner.Seat, sourceSeat, responderSeat, incomingCard, requiredCardKind));
    }

    internal bool CanRequestFactionSlashResponse(CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Slash, CardActionType.Response) || !owner.IsAlive || _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)is null)
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(duel.Attack, attack))
        {
            return duel.ResponderSeat == owner.Seat && !duel.FactionSlashAttempted;
        }

        return !attack.FactionSlashAttempted;
    }

    internal bool CanUseFactionSlashRequest(CharacterState owner, string providerFactionId)
    {
        if (_coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)is not { } policy || policy.FactionId != providerFactionId || global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.All(kind => _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, kind, CardActionType.Use)) || !owner.IsAlive || GetFactionProviderSeats(owner.Seat, providerFactionId).Count == 0)
        {
            return false;
        }

        return _coreMatchParticipantsPlayersAccess._players.Any(target => CanUseProvidedSlashTarget(owner, target));
    }

    internal bool CanUseProvidedSlashTarget(CharacterState owner, CharacterState target, CardKind? effectiveKind = null, Suit? physicalSuit = null, bool allowAnyPhysicalSuit = true, bool? effectiveColor = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null) => effectiveKind is { } kind ? target.IsAlive && target.Seat != owner.Seat && (_coreRuleQueriesOperations.CanSpendSlashUse(owner, target, ignoresCount: false, kind, effectiveRank: effectiveRank, physicalCardIds: physicalCardIds, effectiveSuit: physicalSuit) || allowAnyPhysicalSuit && effectiveRank is null && (_coreJudgedRankSlashPoliciesOperations.HasPotentialJudgedRankSlashQuota(owner.Seat, kind) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianPotential(owner.Seat)) || allowAnyPhysicalSuit && physicalCardIds is null && _characterGuanYuWumiaoOperations.HasPotentialWuyouFactionSlash(owner, kind)) && (_coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.ProvidedSlash, new(owner, target, kind, PhysicalCardIds: physicalCardIds, PhysicalSuit: physicalSuit, EffectiveColor: effectiveColor, EffectiveRank: effectiveRank, AllowAnyPhysicalSuit: allowAnyPhysicalSuit))) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) : global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(candidate => CanUseProvidedSlashTarget(owner, target, candidate, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds));
    internal bool HasEmptyEquipmentSlotForOwnerHandEquipment(CharacterState owner, CharacterState target) => !target.EquipmentAreaAbolished && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(card => _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(target.Seat, card));
    internal IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId) => Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(_coreActualTurnProgressCurrentSeatAccess._currentSeat, offset)).Where(seat => seat != ownerSeat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && (factionId == "magic-any" || string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]), factionId, StringComparison.Ordinal))).ToArray();
    internal bool HasBlackSlashBarrier(CharacterState player) => _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).BlocksBlackSlash) || !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && _coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Armor) == CardKind.RenwangShield || !HasArmor(player) && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.VirtualEquipment, CardKind.RenwangShield);
    internal bool HasArmor(CharacterState player) => _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor);
    internal bool HasArmorBypass(CharacterState player) => _coreAdvancedWeaponsOperations.GetEquipmentRuleCards(player).Any(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).IgnoresArmor);
}
