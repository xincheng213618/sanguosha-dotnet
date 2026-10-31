#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.PhysicalCardDrawRuntime))]
internal sealed class CorePhysicalCardDrawPhysicalCardDrawAdapter(global::System.Func<global::CardGame.Core.PhysicalCardDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawPhysicalCardDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.PhysicalCardDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PhysicalCardDrawRuntime Get() => this._targetFactory();
}
