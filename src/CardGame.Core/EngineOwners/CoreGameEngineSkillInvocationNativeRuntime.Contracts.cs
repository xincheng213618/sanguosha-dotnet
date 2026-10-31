#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSkillInvocationNativeCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false);
}

internal interface ICoreSkillInvocationNativeCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreSkillInvocationNativeCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeFrame(long frameId);
}

internal interface ICoreSkillInvocationNativeCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSkillInvocationNativeCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSkillInvocationNativeKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreSkillInvocationNativeCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreSkillInvocationNativeCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNativeEquipmentInvocation(global::CardGame.Core.SkillInvocationFrame frame);
}

internal interface ICoreSkillInvocationNativeResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSkillInvocationNativeCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreSkillInvocationNativeCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false);
}

internal interface ICoreSkillInvocationNativeCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICoreSkillInvocationNativeCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
    void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreSkillInvocationNativeCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreSkillInvocationNativeCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RestoreFactionRequestAfterQinglongChoice(long ownerId);
}

internal interface ICoreSkillInvocationNativeCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EndTurn();
}

internal interface ICoreSkillInvocationNativeCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}
