#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameHandHpSelectionQuery(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime>> getCoreGameEngineGameTargetHandHpChoicesRuntime) : IGameHandHpSelectionQuery
{
    public bool CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => getCoreGameEngineGameTargetHandHpChoicesRuntime().Value.CanSelectGameHandHpTarget(ownerSeat, skillId, stateId, targetSeat);
}
