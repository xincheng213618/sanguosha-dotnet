#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetWindowProtocolsRuntime
{
    internal ProgramTargetWindowProtocols TargetWindowProtocols => _targetWindowProtocolsOperations._targetWindowProtocols ??= new(CoreProgramTargetWindowProtocolsDefinitions.TargetWindowMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
