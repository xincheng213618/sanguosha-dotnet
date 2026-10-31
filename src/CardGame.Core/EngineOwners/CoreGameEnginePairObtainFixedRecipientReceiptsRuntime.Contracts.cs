#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePairObtainFixedRecipientReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePairObtainFixedRecipientReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICorePairObtainFixedRecipientReceiptsCoreFixedRecipientBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePairObtainFixedRecipientReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePairObtainFixedRecipientReceiptsCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f);
    string PairBenefitMoveReason(global::CardGame.Core.ProgramSkillFrame f, bool gift);
}
