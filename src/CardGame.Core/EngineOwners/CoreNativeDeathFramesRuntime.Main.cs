#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDeathFramesRuntime
{
    internal long BeginDeath(long parentFrameId, int victimSeat, int? killerSeat, DeathReturnKind returnKind)
    {
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new DeathFrame(frameId, parentFrameId, victimSeat, killerSeat) { ReturnKind = returnKind });
        return frameId;
    }
}
