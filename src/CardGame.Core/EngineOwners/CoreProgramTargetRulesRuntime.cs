#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramTargetRulesRuntime(IKernelTargetRulesAccess _targetRulesOperations, IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations) : IProgramTargetQuotaQueries
{
}
