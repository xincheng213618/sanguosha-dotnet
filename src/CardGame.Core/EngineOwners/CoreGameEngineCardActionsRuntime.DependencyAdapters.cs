#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreCardActionsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreCardActionsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreCardActionsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreCardActionsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardActionsCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICoreCardActionsCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}

[Core.ProgramCapability(typeof(CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreCardActionsCoreProgramCardUsePaymentsAdapter(Func<CoreProgramCardUsePaymentsRuntime> targetFactory) : ICoreCardActionsCoreProgramCardUsePaymentsOperations
{
    private readonly Func<CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}
