#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramPaidCardRuntime))]
internal sealed class CoreProgramCardPaymentsProgramPaidCardAdapter(Func<ProgramPaidCardRuntime> targetFactory) : ICoreProgramCardPaymentsProgramPaidCardOperations
{
    private readonly Func<ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public ProgramPaidCardRuntime Get() => this._targetFactory();
}
