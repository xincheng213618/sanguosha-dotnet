#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreModeNativeFrameRuntimeRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreModeNativeFrameRuntimeRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
