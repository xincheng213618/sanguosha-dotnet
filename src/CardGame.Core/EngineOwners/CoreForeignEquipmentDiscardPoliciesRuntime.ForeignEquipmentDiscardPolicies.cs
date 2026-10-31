#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreForeignEquipmentDiscardPoliciesRuntime
{
    internal bool HasForeignDiscardCapability => _characterBianFuRenOlOperations.HasBianFuRenCapability || _characterZhangFenAnniversaryOperations.HasDagongCheCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.PreventForeignEquipmentDiscard) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.XiaHuiBlackHandAndGainedEntityRestrictions) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.SelectEquipmentPairAndPayment) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SelectDyingOwnedCard) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BindBoundaryYuJinRemainder) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RestrictDamageSourceHandCategory);

    internal bool IsForeignEquipmentDiscardPrevented(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => _closedOwnedCardMoveQueriesSource.Get().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.UnupgradedCartDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => _characterZhangFenAnniversaryOperations.IsUnupgradedDagongCheDiscardForbidden(actorSeat, card, from, intent);
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.ChainedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => _characterPangTongAnniversaryGodOperations.IsShenPangTongHandDiscardForbidden(actorSeat, card, from, intent);
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.GiftedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => _characterDongBaiOlOperations.IsXiaHuiDiscardForbidden(actorSeat, card, from, intent);
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.TurnHandCategoryRestricted(int ownerSeat, Card card) => _coreTurnHandCategoryRestrictionsOperations.IsTurnHandCategoryRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IOwnedCardMoveLegacyFacts.HasEquipmentDiscardProtection(int ownerSeat) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.PreventForeignEquipmentDiscard);
    internal bool HasDiscardableHeBy(int actorSeat, int ownerSeat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(actorSeat, card, CardLocation.Hand(ownerSeat), OwnedCardMoveIntent.Discard)) || _corePhysicalOwnedCardQueriesOperations.GetEquipment2(ownerSeat).Any(card => !IsForeignEquipmentDiscardPrevented(actorSeat, card, CardLocation.Equipment(ownerSeat), OwnedCardMoveIntent.Discard));
}
