#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpPangTongNativeFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState,Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreNativeDamageHandlesRuntime>> getCoreNativeDamageHandlesRuntime,Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime,Func<RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries> getCoreGameEngineAlcoholIdentityPoliciesRuntime,Func<RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime,Func<RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime,Func<RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime,Func<RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries> getCoreGameEngineSameNameHandDiscardOrDamageObserversRuntime,Func<FrameStore> getFrameStore) : ISpPangTongNativeFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool UsesFormalWoodenOx => getCoreNativeDamageHandlesRuntime().Value.UsesFormalWoodenOx;
    public bool UsesTreatedWounded => getCoreGameEngineTreatedWoundedRuntime().UsesTreatedWounded;

    public bool IsRuleWounded(CharacterState player) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(player);
    public int EffectiveOwnedCardRank(CharacterState owner, Card card) => getCoreGameEngineAlcoholIdentityPoliciesRuntime().EffectiveOwnedCardRank(owner, card);
    public bool IsSourceEquipment(int owner, string skillId, string instanceId, Card card) => getCoreGameEngineEquipmentSkillsRuntime().IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    public CardLocation NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to) => getCoreGameEngineEphemeralCardMaterialsRuntime().NormalizeEphemeralCardDestination(card, from, to);
    public bool RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(child, parent);
    public bool SameNameObserverSuffix(long rootFrameId) => getCoreGameEngineSameNameHandDiscardOrDamageObserversRuntime().SameNameHandObserverSuffix(getFrameStore().FindIndex(frame => frame.Id == rootFrameId));
}
