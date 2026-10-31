#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreIdentityAppearanceInvariantsCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIdentityAppearanceInvariantsCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool IdentityAppearanceAttackMatches(global::CardGame.Core.IdentityAppearanceReceipt r, global::CardGame.Core.GameEngine.IDamageAttempt a) => this._targetFactory().IdentityAppearanceAttackMatches(r, a);
    public void RequireIdentityAppearance(global::CardGame.Core.IdentityAppearanceFrame frame) => this._targetFactory().RequireIdentityAppearance(frame);
}
