#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public bool IsHappyWeiYanRetainedGainSettlement(global::CardGame.Core.HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, global::CardGame.Core.CardLocation requestedTo, long before) => this._targetFactory().IsHappyWeiYanRetainedGainSettlement(settlement, producerFrameId, cardId, requestedTo, before);
    public bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo) => this._targetFactory().IsHappyWeiYanRedirectedGain(movement, expectedTo);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsTeamMode { get => this._targetFactory().IsTeamMode; }
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCorePrintedLordBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCorePrintedLordBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> _targetFactory = targetFactory;
    public bool ExactPrintedLordStart(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ExactPrintedLordStart(f);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrintedLordChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PrintedLordChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreBlackGiftContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreBlackGiftContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> _targetFactory = targetFactory;
    public bool BlackGiftResultMatches(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.PindianResult result) => this._targetFactory().BlackGiftResultMatches(f, out result);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BlackGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().BlackGiftChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreRecipientConsequencesReceiptsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientConsequencesReceiptsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().IsEphemeralCardDestroyedMaterial(movement);
}
