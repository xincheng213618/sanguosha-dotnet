#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenJwSelection(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime) : IShenJiangWeiSelection
{
    public bool CanPay(int owner) => matchParticipantsState.ReferenceToPlayers[owner].IsAlive && matchParticipantsState.ReferenceToPlayers[owner].HasHpValue && matchParticipantsState.ReferenceToPlayers[owner].MaxHp > 9 && !getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<ShenJiangWeiPingxiangPaidEvent>().Any(p => p.Source.OwnerSeat == owner);
}
