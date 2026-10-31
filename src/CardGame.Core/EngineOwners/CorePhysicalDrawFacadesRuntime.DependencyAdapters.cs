#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime))]
internal sealed class CorePhysicalDrawFacadesCorePhysicalCardDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalDrawFacadesCorePhysicalCardDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PhysicalCardDrawRuntime PhysicalCardDraws { get => this._targetFactory().PhysicalCardDraws; }
}
