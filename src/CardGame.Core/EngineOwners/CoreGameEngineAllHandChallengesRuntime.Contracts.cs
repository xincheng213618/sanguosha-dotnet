#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAllHandChallengesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreAllHandChallengesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreAllHandChallengesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreAllHandChallengesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreAllHandChallengesCoreAllHandChallengeReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidAllHandChallenge(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAllHandChallengesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreAllHandChallengesCoreSequentialDiscardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SequentialDiscardCanContinue(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.CardConversionSource SequentialDiscardSource(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring);
    global::CardGame.Core.SkillProgramStepOutcome FinishSequentialDiscard(global::CardGame.Core.ProgramSkillFrame frame, int remaining);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)> SequentialDiscardCards(global::CardGame.Core.ProgramSkillFrame frame, int chooser, global::CardGame.Core.SkillProgramEffect effect);
    global::CardGame.Core.SkillProgramEffect SequentialDiscardEffect(global::CardGame.Core.ProgramSkillFrame frame);
    void PaySequentialDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> sources);
}

internal interface ICoreAllHandChallengesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreAllHandChallengesCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreAllHandChallengesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreAllHandChallengesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}
