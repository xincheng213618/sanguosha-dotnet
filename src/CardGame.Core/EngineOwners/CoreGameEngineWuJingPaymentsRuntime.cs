#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWuJingPaymentsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries _coreCardUsePaymentsOperations;
    internal CoreGameEngineWuJingPaymentsRuntime(global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries coreCardUsePaymentsOperations)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
    }
}
