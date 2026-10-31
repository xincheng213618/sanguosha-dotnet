#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineIdentityAppearanceInvariantsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    internal CoreGameEngineIdentityAppearanceInvariantsRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
    }
}
