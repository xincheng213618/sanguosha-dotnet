#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DynamicDiscardDamageSelectionQuery(Func<RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries> getCoreGameEngineDynamicDiscardDamagePaymentsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IDynamicDiscardDamageSelectionQuery
{
    public bool CanStart(int ownerSeat, string skillId) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().CanStartDynamicDiscardDamage(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public bool CanTarget(int ownerSeat, string skillId, int targetSeat) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().CanSelectDynamicDiscardDamageTarget(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId, targetSeat);
}
