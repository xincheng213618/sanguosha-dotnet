#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreObserverSubtreesCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreObserverSubtreesCoreCompletedUndamagedTargetRevealObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntBoolOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(global::System.Func<int, bool> __nativeReceiver, int arg2);
}

internal interface ICoreObserverSubtreesCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface ICoreObserverSubtreesCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineCardGameCoreProgramSkillFrameCardGameCoreResolutionFrameBoolOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> __nativeReceiver, global::CardGame.Core.ProgramSkillFrame arg2, global::CardGame.Core.ResolutionFrame arg3);
}

internal interface ICoreObserverSubtreesCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICoreObserverSubtreesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreObserverSubtreesCoreOutsidePhaseDrawDiscardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCharacterPangTongHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyPangTongStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreObserverSubtreesCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDiou(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreObserverSubtreesCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDeathResolution(long deathFrameId);
}

internal interface ICoreObserverSubtreesCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreObserverSubtreesCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyQiaoHealthChoiceRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
    bool HappyQiaoShutuDeclarationEdge(int index);
}

internal interface ICoreObserverSubtreesCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreObserverSubtreesCharacterYanPuOlOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntCardGameCoreDyingFrameBoolOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? __nativeReceiver, int arg2, global::CardGame.Core.DyingFrame arg3);
}

internal interface ICoreObserverSubtreesCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCoreDyingSuitsAndHistoricalUseObserversOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying);
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCharacterMuShunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MuShunPindianStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreObserverSubtreesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeFrame(long frameId);
}

internal interface ICoreObserverSubtreesResolutionStackOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack();
}

internal interface ICoreObserverSubtreesCharacterDiaoChanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyDiaoChanStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreObserverSubtreesCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreOwnedDeathBenefitReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByOwnedDeathBenefit(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
    bool IsPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreObserverSubtreesCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCoreOffTurnUsedCardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByHeJinYanhuo(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreObserverSubtreesCharacterDianWeiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyDianWeiStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreObserverSubtreesCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreObserverSubtreesCoreHalfHandPhaseDebtDamageObserversOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreObserverSubtreesCoreProgramChildResolutionOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}
