#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineIdentityAppearanceInvariantsRuntime
{
    private readonly ICoreIdentityAppearanceInvariantsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreIdentityAppearanceInvariantsCoreIdentityConcealmentOperations _coreIdentityConcealmentOperations;
    internal CoreGameEngineIdentityAppearanceInvariantsRuntime(ICoreIdentityAppearanceInvariantsResolutionStackOperations resolutionStackOperations, ICoreIdentityAppearanceInvariantsCoreIdentityConcealmentOperations coreIdentityConcealmentOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
    }
}
