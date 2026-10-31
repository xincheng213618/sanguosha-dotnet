#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime))]
internal sealed class CorePrepDiscardReceiptsCorePrepDiscardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCorePrepDiscardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> _targetFactory = targetFactory;
    public long PrepDiscardSequence { get => this._targetFactory().PrepDiscardSequence; }

    public bool MatchesActualPrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().MatchesActualPrepDiscard(f);
    public global::CardGame.Core.CardConversionSource PrepDiscardSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PrepDiscardSource(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CorePrepDiscardReceiptsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed class CorePrepDiscardReceiptsCorePrepDiscardEndingPromisesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCorePrepDiscardEndingPromisesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _targetFactory = targetFactory;
    public bool ValidPrepDiscardEndingDraw(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPrepDiscardEndingDraw(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CorePrepDiscardReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime))]
internal sealed class CorePrepDiscardReceiptsCorePrepDiscardImmediateDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCorePrepDiscardImmediateDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> _targetFactory = targetFactory;
    public bool ValidPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidPrepDiscardImmediateDraw(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CorePrepDiscardReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrepDiscardReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}
