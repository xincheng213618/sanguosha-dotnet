#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpPangTongNativeFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries> getCoreGameEngineAlcoholIdentityPoliciesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries> getCoreGameEngineSameNameHandDiscardOrDamageObserversRuntime,global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : ISpPangTongNativeFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool UsesFormalWoodenOx => getCoreNativeDamageHandlesRuntime().UsesFormalWoodenOx;
    public bool UsesTreatedWounded => getCoreGameEngineTreatedWoundedRuntime().UsesTreatedWounded;

    public bool IsRuleWounded(CharacterState player) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(player);
    public int EffectiveOwnedCardRank(CharacterState owner, Card card) => getCoreGameEngineAlcoholIdentityPoliciesRuntime().EffectiveOwnedCardRank(owner, card);
    public bool IsSourceEquipment(int owner, string skillId, string instanceId, Card card) => getCoreGameEngineEquipmentSkillsRuntime().IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    public CardLocation NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to) => getCoreGameEngineEphemeralCardMaterialsRuntime().NormalizeEphemeralCardDestination(card, from, to);
    public bool RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(child, parent);
    public bool SameNameObserverSuffix(long rootFrameId) => getCoreGameEngineSameNameHandDiscardOrDamageObserversRuntime().SameNameHandObserverSuffix(getFrameStore().FindIndex(frame => frame.Id == rootFrameId));
}
