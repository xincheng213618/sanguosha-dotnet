#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientConsequencesReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecipientConsequencesReceiptsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecipientConsequencesReceiptsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanRetainedGainSettlement(global::CardGame.Core.HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, global::CardGame.Core.CardLocation requestedTo, long before);
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICoreRecipientConsequencesReceiptsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTeamMode { get; }

    bool IsNationalWarMode { get; }
}

internal interface ICoreRecipientConsequencesReceiptsCorePrintedLordBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactPrintedLordStart(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrintedLordChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientConsequencesReceiptsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreRecipientConsequencesReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRecipientConsequencesReceiptsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreRecipientConsequencesReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientConsequencesReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecipientConsequencesReceiptsCoreBlackGiftContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BlackGiftResultMatches(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.PindianResult result);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BlackGiftChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientConsequencesReceiptsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}
