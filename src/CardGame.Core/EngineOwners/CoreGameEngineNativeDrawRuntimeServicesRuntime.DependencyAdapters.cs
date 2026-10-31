#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCharacterZhangFeiOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCharacterZhangFeiOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptContext context) => this._targetFactory().NativeDrawPileSlashReplacementProof(frame, context);
    public bool IsIssuedNativeDrawPileSlashReplacement(global::CardGame.Core.NativeDrawAttemptContext context, global::CardGame.Core.NativeDrawReplacementResult replacement) => this._targetFactory().IsIssuedNativeDrawPileSlashReplacement(context, replacement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreOutsidePhaseDrawDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreOutsidePhaseDrawDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _targetFactory = targetFactory;
    public long OutsidePhaseMovementSequence { get => this._targetFactory().OutsidePhaseMovementSequence; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public long PairBenefitSequence { get => this._targetFactory().PairBenefitSequence; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public void CompleteLingkongNativeDrawReplacement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptFrame attempt, global::CardGame.Core.NativeDrawReplacementResult replacement) => this._targetFactory().CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind) => this._targetFactory().GetProgramCardSet(frame, bind);
    public int? ResolveProgramDrawCount(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression) => this._targetFactory().ResolveProgramDrawCount(frame, targetSeat, amount, numberExpression);
    public void DrawProgramSelectedTargets(long frameId, int amount, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramSelectedTargets(frameId, amount, reason);
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
    public void CompleteProgramDrawCards(long frameId, int targetSeat, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason, long fixedRecipientSequenceBefore, global::System.Collections.Generic.IReadOnlyList<int> drawn) => this._targetFactory().CompleteProgramDrawCards(frameId, targetSeat, resultBind, visibility, reason, fixedRecipientSequenceBefore, drawn);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts) => this._targetFactory().CollectEligibleProgramTriggerCandidates(owner, window, facts);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo) => this._targetFactory().IsHappyWeiYanRedirectedGain(movement, expectedTo);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCorePrepDiscardImmediateDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCorePrepDiscardImmediateDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> _targetFactory = targetFactory;
    public bool CanApplyPrepDiscardImmediateDraw(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CanApplyPrepDiscardImmediateDraw(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreNativeDrawAttemptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreNativeDrawAttemptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> _targetFactory = targetFactory;
    public bool HasNativeDrawAttemptCapability { get => this._targetFactory().HasNativeDrawAttemptCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => this._targetFactory().CurrentActualDiscardRecoveryPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreLiushiEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreLiushiEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime> _targetFactory = targetFactory;
    public bool IsLiushiClearDraw(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsLiushiClearDraw(frame);
    public void CaptureLiushiClearDrawPayment(global::CardGame.Core.ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => this._targetFactory().CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed class CoreNativeDrawRuntimeServicesCoreProgramCategoryTargetLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesCoreProgramCategoryTargetLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _targetFactory = targetFactory;
    public int GetProgramPhaseSkillUsage(global::CardGame.Core.ProgramSkillFrame frame, string usageId) => this._targetFactory().GetProgramPhaseSkillUsage(frame, usageId);
}
