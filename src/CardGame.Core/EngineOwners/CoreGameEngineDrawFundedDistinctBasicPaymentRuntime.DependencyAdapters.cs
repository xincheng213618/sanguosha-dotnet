#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterMaTieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterMaTieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryMaTiePaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => this._targetFactory().IsAnniversaryMaTiePaymentFrame(paid);
    public bool TryCancelAnniversaryMaTiePayment(long id) => this._targetFactory().TryCancelAnniversaryMaTiePayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool FinishSayingSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame supplied) => this._targetFactory().FinishSayingSuppliedSlashFailure(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default) => this._targetFactory().ApplyDyingResponse(responder, usePeach, peachCardId, useAlcohol, alcoholCardId, peachConversionSource, alcoholConversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => this._targetFactory().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsCaoChunInitialRequest(f);
    public void FinishCaoChun(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().FinishCaoChun(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveQinglongCrescentBladeChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.DrawFundedBasicNeed? DrawFundedDistinctBasicContext(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ResolutionFrame? suppliedTop = default) => this._targetFactory().DrawFundedDistinctBasicContext(prompt, suppliedTop);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DrawFundedDistinctBasicResponseChoices(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need) => this._targetFactory().DrawFundedDistinctBasicResponseChoices(prompt, need);
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false) => this._targetFactory().DrawFundedDistinctBasicRules(actor, output, responseUse, dyingUse);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicNativePlayActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().DrawFundedDistinctBasicNativePlayActions(actor, source, kind);
    public bool DrawFundedDistinctBasicNativeResponseLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardKind kind, int? targetSeat) => this._targetFactory().DrawFundedDistinctBasicNativeResponseLegal(actor, need, kind, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool FinishWuJingPaidAdapter(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().FinishWuJingPaidAdapter(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterKongRongMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterKongRongMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouKrLiwenPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => this._targetFactory().IsMouKrLiwenPaymentFrame(paid);
    public bool TryCancelMouKrLiwenPayment(long id) => this._targetFactory().TryCancelMouKrLiwenPayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouLuSuPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame p) => this._targetFactory().IsMouLuSuPaymentFrame(p);
    public bool TryCancelMouLuSuPayment(long id) => this._targetFactory().TryCancelMouLuSuPayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsQigongRequest(f);
    public void FinishLvKuangLvXiang(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().FinishLvKuangLvXiang(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreWenyiPeachSuppliesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreWenyiPeachSuppliesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> _targetFactory = targetFactory;
    public void ReserveWenyiDrawFundedPeach(global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().ReserveWenyiDrawFundedPeach(original, need, source, kind);
    public bool WenyiDrawFundedNeedStillLegal(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => this._targetFactory().WenyiDrawFundedNeedStillLegal(paid);
    public bool TryCancelWenyiPeachSupply(long? parentId) => this._targetFactory().TryCancelWenyiPeachSupply(parentId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterLiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterLiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> _targetFactory = targetFactory;
    public bool FinishChanshuangSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().FinishChanshuangSuppliedSlashFailure(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public bool IsKeBiNengPingduanPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => this._targetFactory().IsKeBiNengPingduanPaymentFrame(paid);
    public bool TryCancelKeBiNengPingduanBasicPayment(long programId) => this._targetFactory().TryCancelKeBiNengPingduanBasicPayment(programId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _targetFactory = targetFactory;
    public void ExecuteDrawFundedDistinctBasicPaid(global::CardGame.Core.DrawFundedDistinctBasicFrame frame) => this._targetFactory().ExecuteDrawFundedDistinctBasicPaid(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime))]
internal sealed class CoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDrawFundedDistinctBasicPaymentCoreDrawFundedDistinctBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> _targetFactory = targetFactory;
    public bool IsDrawFundedDistinctBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => this._targetFactory().IsDrawFundedDistinctBasicProgramSelection(parent, effect, paid);
}
