#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CorePairObtainFixedRecipientReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePairObtainFixedRecipientReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CorePairObtainFixedRecipientReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePairObtainFixedRecipientReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed class CorePairObtainFixedRecipientReceiptsCoreFixedRecipientBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePairObtainFixedRecipientReceiptsCoreFixedRecipientBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _targetFactory = targetFactory;
    public bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidFixedRecipientReceipt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CorePairObtainFixedRecipientReceiptsCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePairObtainFixedRecipientReceiptsCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PairBenefitSource(f);
    public string PairBenefitMoveReason(global::CardGame.Core.ProgramSkillFrame f, bool gift) => this._targetFactory().PairBenefitMoveReason(f, gift);
}
