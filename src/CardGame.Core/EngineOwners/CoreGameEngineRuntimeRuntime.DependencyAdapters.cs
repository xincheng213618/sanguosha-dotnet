#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeFrameAutomaticRuntime))]
internal sealed class CoreRuntimeNativeFrameAutomaticAdapter(global::System.Func<global::CardGame.Core.NativeFrameAutomaticRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeNativeFrameAutomaticOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeFrameAutomaticRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeFrameAutomaticRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchStepDispatcher))]
internal sealed class CoreRuntimeMatchStepDispatcherAdapter(global::System.Func<global::CardGame.Core.MatchStepDispatcher> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeMatchStepDispatcherOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchStepDispatcher> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchStepDispatcher Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeFrameRoutingRuntime))]
internal sealed class CoreRuntimeNativeFrameRoutingAdapter(global::System.Func<global::CardGame.Core.NativeFrameRoutingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeNativeFrameRoutingOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeFrameRoutingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeFrameRoutingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramCompletionRuntime))]
internal sealed class CoreRuntimeProgramCompletionAdapter(global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeProgramCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCompletionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreRuntimeCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}
