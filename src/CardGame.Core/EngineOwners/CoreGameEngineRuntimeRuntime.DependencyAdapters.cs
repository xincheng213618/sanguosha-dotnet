#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(NativeFrameAutomaticRuntime))]
internal sealed class CoreRuntimeNativeFrameAutomaticAdapter(Func<NativeFrameAutomaticRuntime> targetFactory) : ICoreRuntimeNativeFrameAutomaticOperations
{
    private readonly Func<NativeFrameAutomaticRuntime> _targetFactory = targetFactory;
    public NativeFrameAutomaticRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MatchStepDispatcher))]
internal sealed class CoreRuntimeMatchStepDispatcherAdapter(Func<MatchStepDispatcher> targetFactory) : ICoreRuntimeMatchStepDispatcherOperations
{
    private readonly Func<MatchStepDispatcher> _targetFactory = targetFactory;
    public MatchStepDispatcher Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(NativeFrameRoutingRuntime))]
internal sealed class CoreRuntimeNativeFrameRoutingAdapter(Func<NativeFrameRoutingRuntime> targetFactory) : ICoreRuntimeNativeFrameRoutingOperations
{
    private readonly Func<NativeFrameRoutingRuntime> _targetFactory = targetFactory;
    public NativeFrameRoutingRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramCompletionRuntime))]
internal sealed class CoreRuntimeProgramCompletionAdapter(Func<ProgramCompletionRuntime> targetFactory) : ICoreRuntimeProgramCompletionOperations
{
    private readonly Func<ProgramCompletionRuntime> _targetFactory = targetFactory;
    public ProgramCompletionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreRuntimeCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreRuntimeCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}
