#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreHalfHandPhaseDebtReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime))]
internal sealed class CoreHalfHandPhaseDebtReceiptsCoreHalfHandPhaseDebtProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsCoreHalfHandPhaseDebtProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource HalfHandDebtSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HalfHandDebtSource(f);
    public bool HalfHandDebtLifecycleMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().HalfHandDebtLifecycleMatches(f, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreHalfHandPhaseDebtReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime))]
internal sealed class CoreHalfHandPhaseDebtReceiptsCoreHalfHandTargetSupportAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsCoreHalfHandTargetSupportOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> _targetFactory = targetFactory;
    public bool HalfHandSupportPaymentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requirePaid) => this._targetFactory().HalfHandSupportPaymentMatches(f, parent, requirePaid);
}
