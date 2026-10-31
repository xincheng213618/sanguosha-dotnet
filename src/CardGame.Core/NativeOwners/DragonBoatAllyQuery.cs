#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DragonBoatAllyQuery(Func<RuntimeEntries.CoreGameEngineDragonBoatGeneralsEntries> getCoreGameEngineDragonBoatGeneralsRuntime) : IDragonBoatAllyQuery
{
    public bool IsTeammate(int ownerSeat, int targetSeat) => ownerSeat != targetSeat && getCoreGameEngineDragonBoatGeneralsRuntime().DragonBoatAllies(ownerSeat, targetSeat);
}
