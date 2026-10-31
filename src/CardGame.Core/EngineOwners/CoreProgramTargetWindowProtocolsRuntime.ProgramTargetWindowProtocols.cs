#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetWindowProtocolsRuntime
{
    internal ProgramTargetWindowProtocols TargetWindowProtocols => _targetWindowProtocolsOperations._targetWindowProtocols ??= new(global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsDefinitions.TargetWindowMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
