#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongOlSpRuntime
{
    Winner ISpPangTongNativeFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool ISpPangTongNativeFacts.UsesFormalWoodenOx => _coreNativeDamageHandlesOperations.UsesFormalWoodenOx;

    bool ISpPangTongNativeFacts.UsesTreatedWounded => _coreTreatedWoundedOperations.UsesTreatedWounded;

    bool ISpPangTongNativeFacts.IsRuleWounded(CharacterState player) => _coreTreatedWoundedOperations.IsRuleWounded(player);
    int ISpPangTongNativeFacts.EffectiveOwnedCardRank(CharacterState owner, Card card) => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(owner, card);
    bool ISpPangTongNativeFacts.IsSourceEquipment(int owner, string skillId, string instanceId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    CardLocation ISpPangTongNativeFacts.NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, to);
    bool ISpPangTongNativeFacts.RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool ISpPangTongNativeFacts.SameNameObserverSuffix(long rootFrameId) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == rootFrameId));
    IReadOnlyList<PromptChoice> ISpPangTongNativeEntries.OwnedCardPaymentChoices(long frameId, int chooser, int owner, IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, zones, intent);
    bool ISpPangTongNativeEntries.BeginPublicHandReveal(long frameId, string returnKey) => _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(frameId, returnKey);
    void ISpPangTongNativeEntries.Recover(long frameId, int owner, int target, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    void ISpPangTongNativeEntries.ChangeMaximumHp(ProgramSkillFrame frame, int amount) => _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, amount);
    void ISpPangTongNativeEntries.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
