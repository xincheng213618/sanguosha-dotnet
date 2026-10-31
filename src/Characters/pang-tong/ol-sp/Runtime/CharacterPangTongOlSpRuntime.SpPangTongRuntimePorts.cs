#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongOlSpRuntime
{
    Winner global::CardGame.Core.ISpPangTongNativeFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.ISpPangTongNativeFacts.UsesFormalWoodenOx => _coreNativeDamageHandlesOperations.UsesFormalWoodenOx;

    bool global::CardGame.Core.ISpPangTongNativeFacts.UsesTreatedWounded => _coreTreatedWoundedOperations.UsesTreatedWounded;

    bool global::CardGame.Core.ISpPangTongNativeFacts.IsRuleWounded(CharacterState player) => _coreTreatedWoundedOperations.IsRuleWounded(player);
    int global::CardGame.Core.ISpPangTongNativeFacts.EffectiveOwnedCardRank(CharacterState owner, Card card) => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(owner, card);
    bool global::CardGame.Core.ISpPangTongNativeFacts.IsSourceEquipment(int owner, string skillId, string instanceId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    bool global::CardGame.Core.ISpPangTongNativeFacts.IsDiouCarrier(int cardId) => _characterDiaoChanLeOperations.IsDiouCarrier(cardId);
    CardLocation global::CardGame.Core.ISpPangTongNativeFacts.NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, to);
    bool global::CardGame.Core.ISpPangTongNativeFacts.RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool global::CardGame.Core.ISpPangTongNativeFacts.SameNameObserverSuffix(long rootFrameId) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == rootFrameId));
    IReadOnlyList<PromptChoice> global::CardGame.Core.ISpPangTongNativeEntries.OwnedCardPaymentChoices(long frameId, int chooser, int owner, IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, zones, intent);
    bool global::CardGame.Core.ISpPangTongNativeEntries.BeginPublicHandReveal(long frameId, string returnKey) => _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(frameId, returnKey);
    void global::CardGame.Core.ISpPangTongNativeEntries.Recover(long frameId, int owner, int target, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    void global::CardGame.Core.ISpPangTongNativeEntries.ChangeMaximumHp(ProgramSkillFrame frame, int amount) => _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, amount);
    void global::CardGame.Core.ISpPangTongNativeEntries.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
