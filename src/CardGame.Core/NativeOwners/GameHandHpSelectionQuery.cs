#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameHandHpSelectionQuery(Func<RuntimeEntries.CoreGameEngineGameTargetHandHpChoicesEntries> getCoreGameEngineGameTargetHandHpChoicesRuntime) : IGameHandHpSelectionQuery
{
    public bool CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => getCoreGameEngineGameTargetHandHpChoicesRuntime().CanSelectGameHandHpTarget(ownerSeat, skillId, stateId, targetSeat);
}
