#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreIdentityAppearanceInvariantsCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IdentityAppearanceAttackMatches(global::CardGame.Core.IdentityAppearanceReceipt r, global::CardGame.Core.GameEngine.IDamageAttempt a);
    void RequireIdentityAppearance(global::CardGame.Core.IdentityAppearanceFrame frame);
}

internal interface ICoreIdentityAppearanceInvariantsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
