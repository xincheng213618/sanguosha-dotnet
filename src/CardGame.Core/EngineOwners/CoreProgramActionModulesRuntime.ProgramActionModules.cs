#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramActionModulesRuntime
{
    internal ProgramActionRouter ProgramActions => _programActionsOperations._programActions ??= new(global::CardGame.Core.EngineOwners.CoreProgramActionModulesDefinitions.ProgramActionMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
