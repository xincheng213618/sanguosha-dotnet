#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramNativeReturnsRuntime
{
    internal ProgramNativeReturns NativeReturns => _nativeReturnsOperations._nativeReturns ??= new(CoreProgramNativeReturnsDefinitions.NativeReturnMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
