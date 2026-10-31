#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRuntimeHooksRuntime
{
    internal ProgramRuntimeHooks RuntimeHooks => _runtimeHooksOperations._runtimeHooks ??= new(CoreProgramRuntimeHooksDefinitions.RuntimeHookMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler, _modeRuntimeOperations._modeRuntime.RuntimeHooks);

    internal void ValidateModeRuntimeHookPriorities() => CoreProgramRuntimeHooksDefinitions.RuntimeHookMetadata.Value.ValidateAdditional(_modeRuntimeOperations._modeRuntime.RuntimeHooks);
}
