#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRicePileReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRicePileReceiptsCoreRicePileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactRiceParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op);
    global::CardGame.Core.CardConversionSource RiceIssuer(global::CardGame.Core.ProgramSkillFrame f);
    bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> RiceStorageCards(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRicePileReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRicePileReceiptsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRicePileReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
