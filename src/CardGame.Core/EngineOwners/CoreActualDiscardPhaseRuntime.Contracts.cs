#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualDiscardPhaseProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreActualDiscardPhaseCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreActualDiscardPhaseCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireCaoChunPlayDebt(int owner, int phaseInstance);
}

internal interface ICoreActualDiscardPhaseCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreActualDiscardPhaseCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat);
}

internal interface ICoreActualDiscardPhaseContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActualDiscardPhaseCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreActualDiscardPhaseCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, global::CardGame.Core.SkillInvocationReturnKind returnKind = (global::CardGame.Core.SkillInvocationReturnKind)0);
}

internal interface ICoreActualDiscardPhaseCharacterLuMengMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPreEntryDiscardPolicy(global::CardGame.Core.CharacterState current);
    bool TryResolvePreEntryDiscardPolicy(bool useSkill);
}

internal interface ICoreActualDiscardPhaseCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EndTurn();
}

internal interface ICoreActualDiscardPhaseCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPlayEndingProgramWindow(global::CardGame.Core.CharacterState owner);
    bool TryBeginDiscardPhaseProgramWindow(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualDiscardPhaseCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreActualDiscardPhaseCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor);
}

internal interface ICoreActualDiscardPhaseCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreActualDiscardPhaseCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentIssuedPhaseHandDebt();
}

internal interface ICoreActualDiscardPhaseCharacterQiaoZhouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySkipQiaoZhouDiscardPhase(global::CardGame.Core.CharacterState current);
}

internal interface ICoreActualDiscardPhaseCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableHand(global::CardGame.Core.CharacterState owner);
    int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualDiscardPhaseKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActualDiscardPhaseCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetHandLimit(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualDiscardPhaseCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentIssuedWeiluDebt(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualDiscardPhaseCharacterDuXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySkipDuXiDiscardPhase(global::CardGame.Core.CharacterState player);
}
