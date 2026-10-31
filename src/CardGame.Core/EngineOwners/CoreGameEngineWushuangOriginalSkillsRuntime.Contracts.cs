#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangOriginalSkillsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreWushuangOriginalSkillsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreWushuangOriginalSkillsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreWushuangOriginalSkillsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreWushuangOriginalSkillsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreWushuangOriginalSkillsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreWushuangOriginalSkillsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreWushuangOriginalSkillsCoreWushuangOriginalSkillFlowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWushuangYiji(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangYashi(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangGuanxu(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangChoufa(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangShenfu(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangLingren(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangTairan(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangTairanSettlement(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueWushuangJieming(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreWushuangOriginalSkillsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWushuangOriginalSkillsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreWushuangOriginalSkillsCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWushuangJinzhi(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreWushuangOriginalSkillsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreWushuangOriginalSkillsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreWushuangOriginalSkillsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreWushuangOriginalSkillsCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreWushuangOriginalSkillsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreWushuangOriginalSkillsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWushuangOriginalSkillsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgram> EnabledSkillPrograms(global::CardGame.Core.CharacterState player);
}

internal interface ICoreWushuangOriginalSkillsCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreWushuangOriginalSkillsCoreWushuangOriginalSkillRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NullifyWushuangCiweiUse(global::CardGame.Core.ProgramSkillFrame f);
    void SkipWushuangFenliPhase(global::CardGame.Core.ProgramSkillFrame f);
    void RestrictWushuangQianxiJudgmentColor(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreWushuangOriginalSkillsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreWushuangOriginalSkillsCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}
