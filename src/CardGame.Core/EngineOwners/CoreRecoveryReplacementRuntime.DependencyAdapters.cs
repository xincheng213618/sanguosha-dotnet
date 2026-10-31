#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramCardUsePaymentsAdapter(Func<CoreProgramCardUsePaymentsRuntime> targetFactory) : ICoreRecoveryReplacementCoreProgramCardUsePaymentsOperations
{
    private readonly Func<CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool ObservesPaidCardUseChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => this._targetFactory().ObservesPaidCardUseChildren(use, continuation);
    public ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}

[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreRecoveryReplacementCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}
