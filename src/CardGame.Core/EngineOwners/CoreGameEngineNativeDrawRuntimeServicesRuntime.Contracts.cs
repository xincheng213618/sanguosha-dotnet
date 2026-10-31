#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDrawRuntimeServicesCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreNativeDrawRuntimeServicesNativeDrawReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnOperation(global::CardGame.Core.SkillProgramEffectOp operation, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome);
}

internal interface ICoreNativeDrawRuntimeServicesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeDrawRuntimeServicesCharacterZhangFeiOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptContext context);
    bool IsIssuedNativeDrawPileSlashReplacement(global::CardGame.Core.NativeDrawAttemptContext context, global::CardGame.Core.NativeDrawReplacementResult replacement);
}

internal interface ICoreNativeDrawRuntimeServicesCoreOutsidePhaseDrawDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long OutsidePhaseMovementSequence { get; }
}

internal interface ICoreNativeDrawRuntimeServicesCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreNativeDrawRuntimeServicesCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long PairBenefitSequence { get; }
}

internal interface ICoreNativeDrawRuntimeServicesCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteLingkongNativeDrawReplacement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptFrame attempt, global::CardGame.Core.NativeDrawReplacementResult replacement);
}

internal interface ICoreNativeDrawRuntimeServicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeDrawRuntimeServicesCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeDrawRuntimeServicesCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreNativeDrawRuntimeServicesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreNativeDrawRuntimeServicesCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeDrawRuntimeServicesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    int? ResolveProgramDrawCount(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression);
    void DrawProgramSelectedTargets(long frameId, int amount, global::CardGame.Core.CardMoveReason reason);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
    void CompleteProgramDrawCards(long frameId, int targetSeat, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason, long fixedRecipientSequenceBefore, global::System.Collections.Generic.IReadOnlyList<int> drawn);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
}

internal interface ICoreNativeDrawRuntimeServicesCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICoreNativeDrawRuntimeServicesCorePrepDiscardImmediateDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanApplyPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreNativeDrawRuntimeServicesProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreNativeDrawRuntimeServicesCoreNativeDrawAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeDrawAttemptCapability { get; }
}

internal interface ICoreNativeDrawRuntimeServicesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreNativeDrawRuntimeServicesCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeDrawRuntimeServicesCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreNativeDrawRuntimeServicesCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
}

internal interface ICoreNativeDrawRuntimeServicesCoreLiushiEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLiushiClearDraw(global::CardGame.Core.ProgramSkillFrame frame);
    void CaptureLiushiClearDrawPayment(global::CardGame.Core.ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced);
}

internal interface ICoreNativeDrawRuntimeServicesCoreProgramCategoryTargetLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramPhaseSkillUsage(global::CardGame.Core.ProgramSkillFrame frame, string usageId);
}
