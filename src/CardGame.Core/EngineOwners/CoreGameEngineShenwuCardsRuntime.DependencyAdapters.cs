#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreShenwuCardsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShenwuCardsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramInteractionRouter ProgramInteractions { get => this._targetFactory().ProgramInteractions; }
}
