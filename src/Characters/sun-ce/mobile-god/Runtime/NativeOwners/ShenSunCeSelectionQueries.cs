#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenSunCeSelectionQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IShenSunCeSelectionQueries
{
    public bool CanSubdue(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive && matchParticipantsState.ReferenceToPlayers[seat].HasHpValue && matchParticipantsState.ReferenceToPlayers[seat].MaxHp > 1;
}
