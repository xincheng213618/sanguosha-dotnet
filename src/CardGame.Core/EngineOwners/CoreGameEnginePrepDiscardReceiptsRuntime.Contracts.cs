#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePrepDiscardReceiptsCorePrepDiscardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long PrepDiscardSequence { get; }

    bool MatchesActualPrepDiscard(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.CardConversionSource PrepDiscardSource(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePrepDiscardReceiptsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICorePrepDiscardReceiptsCorePrepDiscardEndingPromisesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPrepDiscardEndingDraw(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePrepDiscardReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePrepDiscardReceiptsCorePrepDiscardImmediateDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICorePrepDiscardReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
