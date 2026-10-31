#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DynamicDiscardDamageSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries> getCoreGameEngineDynamicDiscardDamagePaymentsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IDynamicDiscardDamageSelectionQuery
{
    public bool CanStart(int ownerSeat, string skillId) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().CanStartDynamicDiscardDamage(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool CanTarget(int ownerSeat, string skillId, int targetSeat) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().CanSelectDynamicDiscardDamageTarget(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId, targetSeat);
}
