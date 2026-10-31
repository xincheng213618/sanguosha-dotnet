#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenJwSelection(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime) : IShenJiangWeiSelection
{
    public bool CanPay(int owner) => matchParticipantsState.ReferenceToPlayers[owner].IsAlive && matchParticipantsState.ReferenceToPlayers[owner].HasHpValue && matchParticipantsState.ReferenceToPlayers[owner].MaxHp > 9 && !getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<ShenJiangWeiPingxiangPaidEvent>().Any(p => p.Source.OwnerSeat == owner);
}
