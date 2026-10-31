#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramCardPaymentsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPaidCardRuntime))]
internal sealed class CoreProgramCardPaymentsProgramPaidCardAdapter(global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardPaymentsProgramPaidCardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPaidCardRuntime Get() => this._targetFactory();
}
