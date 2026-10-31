#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerRuntime))]
internal sealed class CoreTurnDiscardSuitHistoryProgramTriggerAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryProgramTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreTurnDiscardSuitHistoryCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnDiscardSuitHistoryCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}
