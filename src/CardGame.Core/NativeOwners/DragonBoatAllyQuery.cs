#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class DragonBoatAllyQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatGeneralsEntries> getCoreGameEngineDragonBoatGeneralsRuntime) : IDragonBoatAllyQuery
{
    public bool IsTeammate(int ownerSeat, int targetSeat) => ownerSeat != targetSeat && getCoreGameEngineDragonBoatGeneralsRuntime().DragonBoatAllies(ownerSeat, targetSeat);
}
