#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreOutsidePhaseDrawDiscardNativeDrawInvoiceOperations : IProgramRuntimeOperations
{
    NativeDrawInvoiceRuntime Get();
}

internal interface ICoreOutsidePhaseDrawDiscardCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
