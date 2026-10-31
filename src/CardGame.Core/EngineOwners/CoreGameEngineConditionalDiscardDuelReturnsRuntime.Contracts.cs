#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreConditionalDiscardDuelReturnsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreConditionalDiscardDuelReturnsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreConditionalDiscardDuelReturnsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreConditionalDiscardDuelReturnsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreConditionalDiscardDuelReturnsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreConditionalDiscardDuelReturnsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreConditionalDiscardDuelReturnsCoreConditionalDiscardDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ConditionalDuelParentMatches(global::CardGame.Core.ProgramSkillFrame f);
    void FinishConditionalDiscardDuel(global::CardGame.Core.ProgramSkillFrame f, bool issued);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ConditionalDuelChoices(global::CardGame.Core.ProgramSkillFrame f);
}
