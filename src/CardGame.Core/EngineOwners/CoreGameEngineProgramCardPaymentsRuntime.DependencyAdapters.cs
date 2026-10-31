#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramCardPaymentsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[Core.ProgramCapability(typeof(ProgramPaidCardRuntime))]
internal sealed class CoreProgramCardPaymentsProgramPaidCardAdapter(Func<ProgramPaidCardRuntime> targetFactory) : ICoreProgramCardPaymentsProgramPaidCardOperations
{
    private readonly Func<ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public ProgramPaidCardRuntime Get() => this._targetFactory();
}
