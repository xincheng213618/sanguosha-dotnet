#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawFundedDistinctBasicPaymentCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryMaTiePaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
    bool TryCancelAnniversaryMaTiePayment(long id);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishSayingSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame supplied);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICoreDrawFundedDistinctBasicPaymentResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f);
    void FinishCaoChun(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.DrawFundedBasicNeed? DrawFundedDistinctBasicContext(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ResolutionFrame? suppliedTop = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DrawFundedDistinctBasicResponseChoices(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicNativePlayActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind);
    bool DrawFundedDistinctBasicNativeResponseLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardKind kind, int? targetSeat);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishWuJingPaidAdapter(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouKrLiwenPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
    bool TryCancelMouKrLiwenPayment(long id);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame p);
    bool TryCancelMouLuSuPayment(long id);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f);
    void FinishLvKuangLvXiang(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreWenyiPeachSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReserveWenyiDrawFundedPeach(global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind);
    bool WenyiDrawFundedNeedStillLegal(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
    bool TryCancelWenyiPeachSupply(long? parentId);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreDrawFundedDistinctBasicPaymentKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishChanshuangSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKeBiNengPingduanPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
    bool TryCancelKeBiNengPingduanBasicPayment(long programId);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteDrawFundedDistinctBasicPaid(global::CardGame.Core.DrawFundedDistinctBasicFrame frame);
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreDrawFundedDistinctBasicPaymentContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
}
