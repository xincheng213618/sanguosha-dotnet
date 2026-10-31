#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreModeNativeFrameRuntimeRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreModeNativeFrameRuntimeRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
