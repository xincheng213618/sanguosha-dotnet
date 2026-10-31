#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRuntimeHooksRuntime
{
    internal ProgramRuntimeHooks RuntimeHooks => _runtimeHooksOperations._runtimeHooks ??= new(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksDefinitions.RuntimeHookMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler, _modeRuntimeOperations._modeRuntime.RuntimeHooks);

    internal void ValidateModeRuntimeHookPriorities() => global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksDefinitions.RuntimeHookMetadata.Value.ValidateAdditional(_modeRuntimeOperations._modeRuntime.RuntimeHooks);
}
