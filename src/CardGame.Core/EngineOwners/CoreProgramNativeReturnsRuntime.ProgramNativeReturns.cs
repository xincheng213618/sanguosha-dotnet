#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Reflection;
using AttackReceipt = CardGame.Core.GameEngine.AttackCompletionReceipt;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramNativeReturnsRuntime
{
    internal ProgramNativeReturns NativeReturns => _nativeReturnsOperations._nativeReturns ??= new(global::CardGame.Core.EngineOwners.CoreProgramNativeReturnsDefinitions.NativeReturnMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
