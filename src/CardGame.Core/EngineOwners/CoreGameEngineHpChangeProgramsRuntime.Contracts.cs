#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHpChangeProgramsCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueFriendDuelResponse(long id);
    bool IsFriendResponseParent(global::CardGame.Core.FriendDuelResponseFrame f);
}

internal interface ICoreHpChangeProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
    void ReturnRuntimeProgramMovement(long frameId);
    void AdvanceRuntimeFrame(long frameId);
}

internal interface ICoreHpChangeProgramsCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHpChangeProgramsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWumiaoFloodHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame frame);
    void ContinueWumiaoFlood(long id);
}

internal interface ICoreHpChangeProgramsCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardSupplyCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child);
    bool IsCardSupplyCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHpChangeProgramsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreHpChangeProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreHpChangeProgramsCharacterSunJianMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouSunJianOlHp(global::CardGame.Core.HpChangeContext change);
}

internal interface ICoreHpChangeProgramsCoreNativeGroupRecoveryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGroupRecoveryTarget();
}

internal interface ICoreHpChangeProgramsCharacterJiangWeiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByMobileJwShenpei(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHpChangeProgramsCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishVirtualBasicUse(long id);
}

internal interface ICoreHpChangeProgramsCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSoleIncomingRangeDraw(long id);
}

internal interface ICoreHpChangeProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHpChangeProgramsCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHpChangeProgramsCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueColorFireAttackPayment(long useId);
}

internal interface ICoreHpChangeProgramsCoreEquipmentRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueEquipmentRecast(long frameId);
}

internal interface ICoreHpChangeProgramsCoreFactionRequestCostRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePaidFactionRequestCost(long producerId);
}

internal interface ICoreHpChangeProgramsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame frame);
    void ContinueGuanduCardEffect(long id);
}

internal interface ICoreHpChangeProgramsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureYintianRecoveryQualification(global::CardGame.Core.HpChangeContext change);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectYintianRecoveryCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateYintianRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreHpChangeProgramsCoreWushuangSkillsBOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectWushuangBChunlaoLossCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateWushuangBChunlaoLossContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCharacterDongBaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveDongBaiHpDecreased(int seat, int before, int after);
}

internal interface ICoreHpChangeProgramsCoreShenwuNpcProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectShenwuRecoveryCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateShenwuRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreHpChangeProgramsCoreQimeiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureQimeiHpQualification(global::CardGame.Core.HpChangeContext change);
    bool IsQimeiEqualityTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectQimeiHpCandidates(global::CardGame.Core.HpChangeContext c);
    global::CardGame.Core.ProgramSkillWindowContext CreateQimeiHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreHpChangeProgramsCoreResponseCompletionHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardResponseCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child);
    bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHpChangeProgramsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreHpChangeProgramsCoreProgramRecoveryReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearProgramRecoveryApplication(global::CardGame.Core.HpChangeContext change);
}

internal interface ICoreHpChangeProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreHpChangeProgramsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreHpChangeProgramsCharacterWangCanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDenglouNativePayment(long id);
}

internal interface ICoreHpChangeProgramsCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashHpChange(long resumeFrameId, global::CardGame.Core.HpChangeContext change);
}

internal interface ICoreHpChangeProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreHpChangeProgramsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashCapability { get; }
}

internal interface ICoreHpChangeProgramsCoreNianActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectNianRecoveryCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateNianRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreHpChangeProgramsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
    void ContinueRecoveryPaidCardUse(long useId);
}

internal interface ICoreHpChangeProgramsCharacterXiZhiCaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureXianfuRecoveryQualification(global::CardGame.Core.HpChangeContext change);
    bool IsXianfuEchoTrigger(global::CardGame.Core.SkillProgramTrigger trigger);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectXianfuHpCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateXianfuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
    void ContinueDragonBoatCardEffect(long id);
}

internal interface ICoreHpChangeProgramsCoreEndingHistoricalUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueHistoricalEndingPayment(long id);
}

internal interface ICoreHpChangeProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreHpChangeProgramsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreHpChangeProgramsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreHpChangeProgramsCoreWenyiDamageHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnWenyiDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health);
    bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health);
}

internal interface ICoreHpChangeProgramsCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSkillInvocationHealthChild(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
}

internal interface ICoreHpChangeProgramsCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNativeDrawTwo(long id);
}

internal interface ICoreHpChangeProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHpChangeProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreHpChangeProgramsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHpChangeProgramsCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouKurouNativeLoss(global::CardGame.Core.HpChangeContext change);
}

internal interface ICoreHpChangeProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
    void ReturnHappyQiaoDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health);
    void ReturnHappyQiaoCardMaximumHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health);
    bool IsHappyQiaoDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health);
    bool IsHappyQiaoCardMaximumHealthReturn(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.HpChangedTriggerWindowFrame health);
}

internal interface ICoreHpChangeProgramsCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId);
    void ResumeDrawFundedDistinctBasicHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame completed);
    bool IsDrawFundedDistinctBasicHpParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child);
}

internal interface ICoreHpChangeProgramsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePoisonHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child);
    void AssertWenhePoisonFireAttackPayment(global::CardGame.Core.CardUseFrame use);
    bool IsPendingWenhePoisonWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    void AssertWenhePoisonMovementWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    bool IsWenhePoisonFireAttackMovement(global::CardGame.Core.CardUseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame child);
    bool IsOriginalDyingSuspendedByWenhePoison(global::CardGame.Core.DyingFrame original);
}

internal interface ICoreHpChangeProgramsCharacterSunShangxiangSpOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureSpSunShangxiangRecovery(global::CardGame.Core.HpChangeContext change);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePolicyCounterspellPayment(long windowId);
}

internal interface ICoreHpChangeProgramsKernelCoreGameEngineProgramTriggerWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
}

internal interface ICoreHpChangeProgramsCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDianhuRecoveryQualification(global::CardGame.Core.HpChangeContext change);
    bool IsDianhuDrawTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectDianhuHpCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateDianhuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
}

internal interface ICoreHpChangeProgramsCoreHuxinMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueHuxinMirrorPlacement(long id);
}

internal interface ICoreHpChangeProgramsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenyiHpTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectWenyiHpCandidates(global::CardGame.Core.HpChangeContext change);
    global::CardGame.Core.ProgramSkillWindowContext CreateWenyiHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreHpChangeProgramsCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuoSiNativePayment(long id);
}

internal interface ICoreHpChangeProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreHpChangeProgramsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenheCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
    void ContinueWenheCardEffect(long id);
}
