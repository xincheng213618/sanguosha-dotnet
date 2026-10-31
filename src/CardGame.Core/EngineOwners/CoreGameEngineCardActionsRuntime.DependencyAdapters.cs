#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreCardActionsCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public void TryIssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => this._targetFactory().TryIssueTieredRoundResponse(owner, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreCardActionsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool HasWuJingLiubing { get => this._targetFactory().HasWuJingLiubing; }
    public bool HasWuJingHeji { get => this._targetFactory().HasWuJingHeji; }

    public global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary) => this._targetFactory().WuJingLiubingUsedSuit(actorSeat, card, effectiveKind, ids, conversions, ordinary);
    public bool IsWuJingHejiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsWuJingHejiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime))]
internal sealed class CoreCardActionsCoreWuJingQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreWuJingQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime> _targetFactory = targetFactory;
    public bool HasWuJingCompletedUseObserver(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasWuJingCompletedUseObserver(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreCardActionsCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending) => this._targetFactory().BeginNextFangtianHalberdTarget(pending);
    public void ContinueSlashAfterFinalizedTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashAfterFinalizedTargets(attack);
    public void ContinueSlashAfterProgramTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashAfterProgramTargetEffects(attack);
    public void ContinueSlashAfterRedirectPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashAfterRedirectPrograms(attack);
    public void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashAfterResponsePrograms(attack);
    public void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginSlashTargetResolution(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreCardActionsCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public bool IsMouHuCheErUse(long id) => this._targetFactory().IsMouHuCheErUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed class CoreCardActionsCoreCurrentTurnUseKindsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCurrentTurnUseKindsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _targetFactory = targetFactory;
    public bool TracksCurrentTurnUseKinds { get => this._targetFactory().TracksCurrentTurnUseKinds; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreCardActionsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool HasSiruiCapability { get => this._targetFactory().HasSiruiCapability; }

    public int? SiruiResponseRank(global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => this._targetFactory().SiruiResponseRank(source, costs);
    public void BindSiruiResponseAction(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindSiruiResponseAction(parent, action);
    public bool TryGetSiruiDestroyedResponseCost(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
    public bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed class CoreCardActionsCoreProgramCardUseNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramCardUseNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _targetFactory = targetFactory;
    public void AssertProgramUseNullification(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertProgramUseNullification(use);
    public bool IsExactSimpleBeforeTargetWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsExactSimpleBeforeTargetWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreCardActionsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public void CaptureLeCaiYongFeibaiQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureLeCaiYongFeibaiQualification(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreCardActionsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public bool HasBianFuRenCapability { get => this._targetFactory().HasBianFuRenCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreCardActionsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouLuSuPlayedWindow(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().IsMouLuSuPlayedWindow(action, window);
    public global::CardGame.Core.MouLuSuAnniversaryPlayedReturn? MouLuSuPlayedWindowReturn(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().MouLuSuPlayedWindowReturn(action, window);
    public bool TryReturnMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryReturnMouLuSuPlayedWindow(window);
    public bool ValidMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidMouLuSuPlayedWindow(window);
    public bool IsMouLuSuPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsMouLuSuPlayedCostFinished(w, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime))]
internal sealed class CoreCardActionsCoreForeignSelectedCardSlashReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreForeignSelectedCardSlashReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> _targetFactory = targetFactory;
    public bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => this._targetFactory().IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime))]
internal sealed class CoreCardActionsCoreActualUseTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreActualUseTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.JizhiResolution pending) => this._targetFactory().TryBeginActualUseTargetPrograms(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreCardActionsCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouDanchiSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasMouDanchiSuspendedCompletedParent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime))]
internal sealed class CoreCardActionsCoreGuanduEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreGuanduEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => this._targetFactory().IsGuanduDestroyedChariotMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCardActionsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreCardActionsCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyZuoCiDestroyedResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish) => this._targetFactory().IsHappyZuoCiDestroyedResponseFinish(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed class CoreCardActionsCoreNativeActionDispatchAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeActionDispatchOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _targetFactory = targetFactory;
    public void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => this._targetFactory().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime))]
internal sealed class CoreCardActionsCharacterHuBanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuBanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _targetFactory = targetFactory;
    public bool HasHuiyunCompletedUseObserver(long useId) => this._targetFactory().HasHuiyunCompletedUseObserver(useId);
    public bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsHuiyunOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreCardActionsCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public int? CaptureAlcoholActionRank(int providerSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().CaptureAlcoholActionRank(providerSeat, kind, ids);
    public int? CaptureAlcoholResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => this._targetFactory().CaptureAlcoholResponseRank(provider, kind, costs);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreCardActionsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool IsRoundOnceCardlessBasicUse(long id) => this._targetFactory().IsRoundOnceCardlessBasicUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreCardActionsCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasCardResponseCompletedObserver(action, continuation);
    public bool TryDrainCardResponseCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => this._targetFactory().TryDrainCardResponseCompletionCosts(supplied);
    public void ContinueCardResponseAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ContinueCardResponseAfterCompletion(attack, window);
    public bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidResponseCompletionWindow(window);
    public bool ResponseCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().ResponseCompletionFirstChild(window, child);
    public bool IsResponseCompletionCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsResponseCompletionCostFinished(window, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime))]
internal sealed class CoreCardActionsCharacterYuanTanYuanShangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterYuanTanYuanShangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> _targetFactory = targetFactory;
    public bool HasNeifaCapability { get => this._targetFactory().HasNeifaCapability; }

    public void CaptureNeifaPaidEquipmentUse(long useId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureNeifaPaidEquipmentUse(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime))]
internal sealed class CoreCardActionsCoreDragonBoatActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDragonBoatActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsDragonBoatActivity { get => this._targetFactory().IsDragonBoatActivity; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreCardActionsCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool HasTargetedOrdinaryTrickAdjustment { get => this._targetFactory().HasTargetedOrdinaryTrickAdjustment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime))]
internal sealed class CoreCardActionsCoreOverflowTargetCancellationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreOverflowTargetCancellationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> _targetFactory = targetFactory;
    public bool TryCompleteOverflowCanceledUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryCompleteOverflowCanceledUse(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreCardActionsCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedPolicyForAction(long actionId) => this._targetFactory().HasIssuedPolicyForAction(actionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreCardActionsCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
    public bool? CaptureActionColor(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.Suit? effectiveSuit = default) => this._targetFactory().CaptureActionColor(costs, effectiveSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreCardActionsCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiGyCompletedUseObserver(long id) => this._targetFactory().HasShiGyCompletedUseObserver(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CoreCardActionsCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => this._targetFactory().HasSkillRoleQualification(owner, skillId, instance, role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreCardActionsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public void CaptureMoQuanyuSingleton(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.SkillProgramTriggerWindow kind) => this._targetFactory().CaptureMoQuanyuSingleton(window, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime))]
internal sealed class CoreCardActionsCharacterTaishiCiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterTaishiCiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryCompleteShenTaishiCiCancelledSlash(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryCompleteShenTaishiCiCancelledSlash(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime))]
internal sealed class CoreCardActionsCharacterZhenJiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhenJiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyFeifuClaimedResponseEntity(global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().IsHappyFeifuClaimedResponseEntity(action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed class CoreCardActionsCharacterRuanYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterRuanYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _targetFactory = targetFactory;
    public bool HasRuanYuCapability { get => this._targetFactory().HasRuanYuCapability; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> IncludeRuanYuMiaoxianRedCandidates(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> ordinary, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().IncludeRuanYuMiaoxianRedCandidates(ordinary, action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreCardActionsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreCardActionsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool TryStartLeChengyanEffect(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryStartLeChengyanEffect(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime))]
internal sealed class CoreCardActionsCoreActorHandLimitTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreActorHandLimitTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> _targetFactory = targetFactory;
    public void RecordActorHandLimitTargetAnnouncements(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().RecordActorHandLimitTargetAnnouncements(action, window);
    public bool ShouldCollectActorHandLimitPenalty(global::CardGame.Core.CardActionContext action, int ownerSeat, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().ShouldCollectActorHandLimitPenalty(action, ownerSeat, window);
    public global::CardGame.Core.ProgramCardTriggerWindowFrame AppendActorHandLimitTargetCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().AppendActorHandLimitTargetCandidates(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreCardActionsCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool TryDrainCardSupplyCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => this._targetFactory().TryDrainCardSupplyCompletionCosts(supplied);
    public void ContinueCardSupplyAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ContinueCardSupplyAfterCompletion(attack, window);
    public bool ValidCardSupplyCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidCardSupplyCompletionWindow(window);
    public bool CardSupplyCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CardSupplyCompletionFirstChild(window, child);
    public bool IsCardSupplyCompletionCostPaid(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsCardSupplyCompletionCostPaid(window, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreCardActionsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreCardActionsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed class CoreCardActionsCoreFriendEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFriendEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _targetFactory = targetFactory;
    public void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action) => this._targetFactory().CaptureFriendMinimumHandUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreCardActionsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreCardActionsCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasConvertedSkillInvocations(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().HasConvertedSkillInvocations(action, window);
    public bool TryBeginConvertedSkillInvocations(global::CardGame.Core.ProgramCardTriggerWindowFrame f) => this._targetFactory().TryBeginConvertedSkillInvocations(f);
    public bool IsCardWindowWaitingForSkillInvocation(global::CardGame.Core.ProgramCardTriggerWindowFrame window, int index) => this._targetFactory().IsCardWindowWaitingForSkillInvocation(window, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed class CoreCardActionsCoreSoleTargetRedirectAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSoleTargetRedirectOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _targetFactory = targetFactory;
    public bool TryBeginSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().TryBeginSoleTargetRedirect(frame);
    public bool TryReturnSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().TryReturnSoleTargetRedirect(frame);
    public bool IsExactSoleTargetRedirectReturn(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().IsExactSoleTargetRedirectReturn(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreCardActionsCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreCardActionsCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasBlackTrickTargetPolicy { get => this._targetFactory().HasBlackTrickTargetPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed class CoreCardActionsCoreEndingHistoricalUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreEndingHistoricalUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _targetFactory = targetFactory;
    public bool TracksOwnPlayHistory { get => this._targetFactory().TracksOwnPlayHistory; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed class CoreCardActionsCoreForeignContestAidObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreForeignContestAidObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeAidTargetObserver(long useId) => this._targetFactory().HasSameTypeAidTargetObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreCardActionsCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public bool HasGuoSiCapability { get => this._targetFactory().HasGuoSiCapability; }

    public void CaptureGuoSiFinalizedUse(long id, global::CardGame.Core.CardActionContext a) => this._targetFactory().CaptureGuoSiFinalizedUse(id, a);
    public void CaptureGuoSiCompletedUse(long id, global::CardGame.Core.CardActionContext a) => this._targetFactory().CaptureGuoSiCompletedUse(id, a);
    public bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original) => this._targetFactory().IsSuspendedGuoSiCompletedParent(original);
    public bool GuoSiNativeRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => this._targetFactory().GuoSiNativeRidesOn(f, use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime))]
internal sealed class CoreCardActionsCoreFirstSlashDuelLegacyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFirstSlashDuelLegacyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardTriggerWindowFrame DeferFirstSlashDuelCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().DeferFirstSlashDuelCandidates(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreCardActionsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public bool IsShanheStartingResourceResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish) => this._targetFactory().IsShanheStartingResourceResponseFinish(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed class CoreCardActionsCoreVirtualOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreVirtualOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public bool MatchesVirtualOrdinaryTrickUseAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesVirtualOrdinaryTrickUseAction(frameId, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreCardActionsCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public bool IsFirstCategoryUse(int owner, string skill, global::CardGame.Core.CardActionContext action) => this._targetFactory().IsFirstCategoryUse(owner, skill, action);
    public int[] AvailableCompletedTopCosts(global::CardGame.Core.CardActionContext a, long parentFrameId) => this._targetFactory().AvailableCompletedTopCosts(a, parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime))]
internal sealed class CoreCardActionsCoreFirstSlashDuelBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFirstSlashDuelBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> _targetFactory = targetFactory;
    public bool CanCollectFirstSlashDuelCandidate(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action) => this._targetFactory().CanCollectFirstSlashDuelCandidate(owner, trigger, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreCardActionsCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasShenDaSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().HasShenDaSuspendedCompletedParent(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed class CoreCardActionsCharacterZhouCangBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhouCangBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryZhouCangCompletedUseObserver(long useId) => this._targetFactory().HasBoundaryZhouCangCompletedUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime))]
internal sealed class CoreCardActionsCoreCompletedGiftQinglongRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCompletedGiftQinglongRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> _targetFactory = targetFactory;
    public bool IsUnselectedCompletedGiftQinglongRide(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsUnselectedCompletedGiftQinglongRide(frame);
    public bool TryGetCompletedGiftRecipientSlashRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window, out global::CardGame.Core.CardUseFrame child) => this._targetFactory().TryGetCompletedGiftRecipientSlashRide(window, out child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime))]
internal sealed class CoreCardActionsCoreCurrentUseTargetCandidateRefreshAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCurrentUseTargetCandidateRefreshOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUseTargetCandidateRefreshRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame NormalizeImplicitDrawTwoUseTarget(global::CardGame.Core.CardUseFrame use) => this._targetFactory().NormalizeImplicitDrawTwoUseTarget(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime))]
internal sealed class CoreCardActionsCoreDelayedPlacementTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDelayedPlacementTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _targetFactory = targetFactory;
    public void ContinueFinalizedDelayedPlacement(global::CardGame.Core.ProgramCardTriggerWindowFrame completedWindow) => this._targetFactory().ContinueFinalizedDelayedPlacement(completedWindow);
    public bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsExactFinalizedDelayedPlacementWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreCardActionsCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public void CaptureMouLiuChengCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureMouLiuChengCompletion(action, window);
    public void CaptureMouLiuChengAccrual(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureMouLiuChengAccrual(action, window);
    public global::CardGame.Core.ProgramSkillWindowContext? AttachMouLiuChengAccrual(global::CardGame.Core.CardActionContext action, string skill, string instance, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().AttachMouLiuChengAccrual(action, skill, instance, context);
    public global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouLiuChengAccrual(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().AppendMouLiuChengAccrual(frame);
    public bool MatchesMouLiuChengNativeAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesMouLiuChengNativeAction(id, action, cardId);
    public bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsMouLiuChengSuspendedCompletedAttack(original, requiredWindowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime))]
internal sealed class CoreCardActionsCharacterPangTongMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterPangTongMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouPtLateTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().AppendMouPtLateTargets(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime))]
internal sealed class CoreCardActionsCharacterSuFeiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterSuFeiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime> _targetFactory = targetFactory;
    public bool HasSuFeiCapability { get => this._targetFactory().HasSuFeiCapability; }

    public void CaptureSuFeiFinalizedUse(long nativeUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureSuFeiFinalizedUse(nativeUseId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed class CoreCardActionsCharacterXuHuangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterXuHuangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _targetFactory = targetFactory;
    public bool MatchesMouXuHuangNativeAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesMouXuHuangNativeAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreCardActionsCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool MatchesMouDcNativeAction(long id, global::CardGame.Core.CardActionContext a, int card) => this._targetFactory().MatchesMouDcNativeAction(id, a, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime))]
internal sealed class CoreCardActionsCoreUnconvertedCardUseQualificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreUnconvertedCardUseQualificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime> _targetFactory = targetFactory;
    public bool IsUnconvertedActualCardUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.ProgramCardContinuation? continuation) => this._targetFactory().IsUnconvertedActualCardUse(action, window, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime))]
internal sealed class CoreCardActionsCoreCompletedUndamagedTargetRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCompletedUndamagedTargetRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _targetFactory = targetFactory;
    public bool CanOfferCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramTriggerLegalityContext gate) => this._targetFactory().CanOfferCompletedUndamagedTargetReveal(gate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CoreCardActionsCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureFactionAction(action);
    public void RecordFactionRewardOffers(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => this._targetFactory().RecordFactionRewardOffers(action, candidates);
    public int? FactionActionRewardChooser(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action) => this._targetFactory().FactionActionRewardChooser(owner, trigger, action);
    public bool WasFactionRewardOffered(int owner, string skillId, global::CardGame.Core.CardActionContext action) => this._targetFactory().WasFactionRewardOffered(owner, skillId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuZglReturnedCopyCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord? finish = default) => this._targetFactory().IsWuZglReturnedCopyCost(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CoreCardActionsCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public bool TracksRoundGainedSourceUses { get => this._targetFactory().TracksRoundGainedSourceUses; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreCardActionsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreCardActionsCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public bool HasSoleTargetProtectionCapability { get => this._targetFactory().HasSoleTargetProtectionCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed class CoreCardActionsCoreChainedStateBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreChainedStateBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _targetFactory = targetFactory;
    public bool IsChainedStateQinglongCompletedWindowRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsChainedStateQinglongCompletedWindowRide(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime))]
internal sealed class CoreCardActionsCharacterZhaoYunOlGod2016Adapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhaoYunOlGod2016Operations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureHpScaledCardUseAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureHpScaledCardUseAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime))]
internal sealed class CoreCardActionsCoreOverflowTargetCancellationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreOverflowTargetCancellationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? OverflowTargetCancellationObserverRoot() => this._targetFactory().OverflowTargetCancellationObserverRoot();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreCardActionsCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool HasCuiYanCapability { get => this._targetFactory().HasCuiYanCapability; }

    public bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit) => this._targetFactory().IsCuiYanNextUseForbidden(actorSeat, materialIds, nativeEffectiveSuit);
    public bool TryReturnCuiYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().TryReturnCuiYanLegacyAlcohol(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime))]
internal sealed class CoreCardActionsCharacterHuLieMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuLieMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> _targetFactory = targetFactory;
    public bool IsMouHuLieUse(long id) => this._targetFactory().IsMouHuLieUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime))]
internal sealed class CoreCardActionsCoreForeignPublicPileSlashesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreForeignPublicPileSlashesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> _targetFactory = targetFactory;
    public bool IsForeignPublicPileSlashUse(long resolutionId) => this._targetFactory().IsForeignPublicPileSlashUse(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreCardActionsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
    public void ContinueJizhiOrNullificationAfterTargetTriggers(global::CardGame.Core.NativeOwners.JizhiResolution pending) => this._targetFactory().ContinueJizhiOrNullificationAfterTargetTriggers(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreCardActionsCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHzIneffectiveResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasAnniversaryHzIneffectiveResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed class CoreCardActionsCoreSameTypeActualUseAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSameTypeActualUseAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeActualUseAid { get => this._targetFactory().HasSameTypeActualUseAid; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreCardActionsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieCompletedUseObserver(long useId) => this._targetFactory().HasChenglieCompletedUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed class CoreCardActionsCharacterZhangYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhangYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiZySuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasShiZySuspendedCompletedParent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreCardActionsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime))]
internal sealed class CoreCardActionsCoreDragonBoatGeneralsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDragonBoatGeneralsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> _targetFactory = targetFactory;
    public void CaptureDragonBoatFinalizedUse(long useId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureDragonBoatFinalizedUse(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreCardActionsCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool HasHuaManCapability { get => this._targetFactory().HasHuaManCapability; }

    public void CaptureHuaManFinalizedUse(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureHuaManFinalizedUse(id, action);
    public void CaptureHuaManCompletedUse(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureHuaManCompletedUse(id, action);
    public global::CardGame.Core.ProgramSkillWindowContext CreateHuaManCardUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateHuaManCardUseContext(frame, c, context);
    public bool HasHuaManCardObserver(long useId) => this._targetFactory().HasHuaManCardObserver(useId);
    public bool IsHuaManSouyingEmptyTargetWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsHuaManSouyingEmptyTargetWindow(window);
    public bool IsHuaManClaimedEntity(long useId, int cardId) => this._targetFactory().IsHuaManClaimedEntity(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime))]
internal sealed class CoreCardActionsCoreCurrentTurnShownCardBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCurrentTurnShownCardBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasShownCardTurnCapability { get => this._targetFactory().HasShownCardTurnCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime))]
internal sealed class CoreCardActionsCoreRoundGainedEquipmentObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreRoundGainedEquipmentObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> _targetFactory = targetFactory;
    public bool HasRoundGainedEquipmentDrawCardObserver(long useId) => this._targetFactory().HasRoundGainedEquipmentDrawCardObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed class CoreCardActionsCharacterMaMiDiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterMaMiDiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _targetFactory = targetFactory;
    public bool HasMaMiDiCapability { get => this._targetFactory().HasMaMiDiCapability; }

    public global::CardGame.Core.ProgramSkillWindowContext CreateMaMiDiIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => this._targetFactory().CreateMaMiDiIssuedContext(w, candidate, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CoreCardActionsCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasRankSlashRange(actor, kind);
    public int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default) => this._targetFactory().ZhangbaSpecificSlashRank(actor, cards, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime))]
internal sealed class CoreCardActionsCorePaidColorConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePaidColorConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> _targetFactory = targetFactory;
    public bool TracksPaidColorUseAppearance(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> chain) => this._targetFactory().TracksPaidColorUseAppearance(actorSeat, kind, chain);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CoreCardActionsCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasLiFengCapability { get => this._targetFactory().HasLiFengCapability; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectJiyunCompletedUseCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation? continuation) => this._targetFactory().CollectJiyunCompletedUseCandidates(action, continuation);
    public global::CardGame.Core.ProgramSkillWindowContext CreateJiyunCompletedUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame mother, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary) => this._targetFactory().CreateJiyunCompletedUseContext(mother, candidate, ordinary);
    public bool IsJiyunClaimedUseEntity(long nativeUseId, int cardId) => this._targetFactory().IsJiyunClaimedUseEntity(nativeUseId, cardId);
    public bool IsJiyunClaimedResponseEntity(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind, int cardId) => this._targetFactory().IsJiyunClaimedResponseEntity(action, kind, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime))]
internal sealed class CoreCardActionsCoreFulueQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFulueQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime> _targetFactory = targetFactory;
    public bool HasFulueCompletedCounterChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasFulueCompletedCounterChild(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime))]
internal sealed class CoreCardActionsCharacterZhangXiuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhangXiuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZhangXiuAnniversaryUse(long id) => this._targetFactory().IsMouZhangXiuAnniversaryUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime))]
internal sealed class CoreCardActionsCoreProgramConversionPolarityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramConversionPolarityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId) => this._targetFactory().GetProgramConversionPolarity(ownerSeat, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime))]
internal sealed class CoreCardActionsCoreShanheLeizhenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreShanheLeizhenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> _targetFactory = targetFactory;
    public void RecordShanheLeizhenCommittedUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordShanheLeizhenCommittedUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed class CoreCardActionsCharacterLiuYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiuYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _targetFactory = targetFactory;
    public bool HasLiuYanCapability { get => this._targetFactory().HasLiuYanCapability; }

    public bool TryReturnLiuYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().TryReturnLiuYanLegacyAlcohol(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreCardActionsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public bool HasMoLuBu { get => this._targetFactory().HasMoLuBu; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuyouCapability { get => this._targetFactory().HasWuyouCapability; }
    public bool HasWuGuanYuJuewuCapability { get => this._targetFactory().HasWuGuanYuJuewuCapability; }

    public int? CaptureJuewuActionRank(int providerSeat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().CaptureJuewuActionRank(providerSeat, ids);
    public int? CaptureJuewuResponseRank(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => this._targetFactory().CaptureJuewuResponseRank(provider, costs);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreCardActionsCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordActualPlayPhaseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreCardActionsCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public bool TryReturnMouBingcaiLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryReturnMouBingcaiLegacyAlcohol(window);
    public bool IsMouLixianRemovedMaterial(long useId, int cardId) => this._targetFactory().IsMouLixianRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreCardActionsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreCardActionsCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool MatchesTieredRoundZeroUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesTieredRoundZeroUseAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreCardActionsCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasYuanchouCardPolicy { get => this._targetFactory().HasYuanchouCardPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreCardActionsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsExchangedCardClaim(long actionId, int cardId) => this._targetFactory().IsExchangedCardClaim(actionId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime))]
internal sealed class CoreCardActionsCoreJoinedTrickDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreJoinedTrickDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HasJoinedTrickDamageRewardCardObserver(long useId) => this._targetFactory().HasJoinedTrickDamageRewardCardObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreCardActionsCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public bool HasMouZuifengCapability { get => this._targetFactory().HasMouZuifengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreCardActionsCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public bool IsWushuangShopDestroyedNullificationPayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material) => this._targetFactory().IsWushuangShopDestroyedNullificationPayment(action, windowId, material);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed class CoreCardActionsCoreSourceCursesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSourceCursesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _targetFactory = targetFactory;
    public bool HasSourceCurseActionColors { get => this._targetFactory().HasSourceCurseActionColors; }

    public global::CardGame.Core.SourceCurseDeposit? EligibleSourceCurseUse(int owner, string skill, string instance, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().EligibleSourceCurseUse(owner, skill, instance, action, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime))]
internal sealed class CoreCardActionsCoreModeCardRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreModeCardRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> _targetFactory = targetFactory;
    public bool CanModeHandEquipmentRecast(global::CardGame.Core.Card card) => this._targetFactory().CanModeHandEquipmentRecast(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime))]
internal sealed class CoreCardActionsCoreCommittedResponseUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCommittedResponseUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _targetFactory = targetFactory;
    public bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().TryBeginCommittedResponseUsePrograms(attack, action, continuation);
    public void ContinueCommittedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().ContinueCommittedResponseUse(attack, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCardActionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreCardActionsCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public bool MatchesOriginalUseLightningAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesOriginalUseLightningAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreCardActionsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime))]
internal sealed class CoreCardActionsCoreHellDizangProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreHellDizangProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> _targetFactory = targetFactory;
    public bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().CanHellDitingMountRecast(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreCardActionsCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public void ValidateOwnPublicPileConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().ValidateOwnPublicPileConversionCommit(source, provider, materials, forResponse, effectiveKind);
    public global::CardGame.Core.Card? OwnPublicPileResponseMaterial(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().OwnPublicPileResponseMaterial(costs, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime))]
internal sealed class CoreCardActionsCoreQuotaTopProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreQuotaTopProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _targetFactory = targetFactory;
    public void RegisterFirstTurnTargetActions(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().RegisterFirstTurnTargetActions(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime))]
internal sealed class CoreCardActionsCoreOffTurnUsedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreOffTurnUsedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> _targetFactory = targetFactory;
    public void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureOffTurnResponseUseOrigin(owningUseId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreCardActionsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
    public global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureAttackCompletion(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime))]
internal sealed class CoreCardActionsCharacterMengDaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterMengDaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime> _targetFactory = targetFactory;
    public bool HasGoudeCompletedSlashChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasGoudeCompletedSlashChild(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreCardActionsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool IsZhangYanSujiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsZhangYanSujiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreCardActionsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyWeiYanIneffectiveResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasHappyWeiYanIneffectiveResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreCardActionsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureProgramTriggerFacts(owner, action);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreCardActionsCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public bool IsZongluanUse(long id) => this._targetFactory().IsZongluanUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime))]
internal sealed class CoreCardActionsCoreSameNameHandDiscardOrDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSameNameHandDiscardOrDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HasSameNameHandCardObserver(long parentFrameId) => this._targetFactory().HasSameNameHandCardObserver(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime))]
internal sealed class CoreCardActionsCoreComparedBlackSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreComparedBlackSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasComparedBlackSlashPolicies { get => this._targetFactory().HasComparedBlackSlashPolicies; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreCardActionsCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateBiguoTargetContext(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary) => this._targetFactory().CreateBiguoTargetContext(window, candidate, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreCardActionsCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsProgramResponseCardUse(responder, effectiveKind);
    public void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().RecordProgramUsedBasicCard(actorSeat, effectiveKind);
    public bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => this._targetFactory().IsProgramAlternativeCost(action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime))]
internal sealed class CoreCardActionsCoreUniqueLeaderTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreUniqueLeaderTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool TracksUniqueLeaderTrickTargets { get => this._targetFactory().TracksUniqueLeaderTrickTargets; }

    public void CaptureUniqueLeaderTrickQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureUniqueLeaderTrickQualification(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreCardActionsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource? GetSelectedUseConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().GetSelectedUseConversion(actor, provider, card, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreCardActionsCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasTurnRedSlashCapability { get => this._targetFactory().HasTurnRedSlashCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreCardActionsCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool IsZonghuUse(long id) => this._targetFactory().IsZonghuUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed class CoreCardActionsCoreDrawFundedDistinctBasicUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDrawFundedDistinctBasicUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsIssuedDrawFundedDistinctBasicUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreCardActionsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime))]
internal sealed class CoreCardActionsCharacterWuAnGuoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterWuAnGuoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> _targetFactory = targetFactory;
    public bool HasWuAnGuoCapability { get => this._targetFactory().HasWuAnGuoCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed class CoreCardActionsCoreEquipmentRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreEquipmentRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _targetFactory = targetFactory;
    public void BeginEquipmentRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().BeginEquipmentRecast(actor, card, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime))]
internal sealed class CoreCardActionsCharacterZhaoXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhaoXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> _targetFactory = targetFactory;
    public bool CanOfferMeiYingConversionDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanOfferMeiYingConversionDraw(candidate, trigger, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime))]
internal sealed class CoreCardActionsCorePairedColorResponseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePairedColorResponseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> _targetFactory = targetFactory;
    public bool HasPairedColorDispositionCardObserver(long parentId) => this._targetFactory().HasPairedColorDispositionCardObserver(parentId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreCardActionsCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation CapturedDeclarationOrigin(int cardId, global::CardGame.Core.CardLocation actual) => this._targetFactory().CapturedDeclarationOrigin(cardId, actual);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuZhangFeiUseCapability { get => this._targetFactory().HasWuZhangFeiUseCapability; }

    public global::CardGame.Core.CardActionContext CaptureWuZhangFeiUseRankAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureWuZhangFeiUseRankAction(action);
    public int? CaptureWuZhangFeiResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().CaptureWuZhangFeiResponseRank(provider, kind, costs, source);
    public void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action) => this._targetFactory().BindWuZhangFeiResponseRanks(action);
    public void CaptureWuZhangFeiUseQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureWuZhangFeiUseQualification(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreCardActionsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => this._targetFactory().ActiveFangtianHalberd; set => this._targetFactory().ActiveFangtianHalberd = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreCardActionsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => this._targetFactory().IsCurrentUsePhysicalCardClaim(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreCardActionsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime))]
internal sealed class CoreCardActionsCharacterWangYunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterWangYunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> _targetFactory = targetFactory;
    public bool HasWangYunPlanCapability { get => this._targetFactory().HasWangYunPlanCapability; }
    public bool HasWangYunTrickCapability { get => this._targetFactory().HasWangYunTrickCapability; }

    public void CompleteWangYunNativePayment(long useId) => this._targetFactory().CompleteWangYunNativePayment(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime))]
internal sealed class CoreCardActionsCharacterYuanShaoMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterYuanShaoMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime> _targetFactory = targetFactory;
    public void CaptureMouYuanShaoDesignation(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureMouYuanShaoDesignation(action, window);
    public global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouYuanShaoTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().AppendMouYuanShaoTargets(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime))]
internal sealed class CoreCardActionsCoreGuanduChariotResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreGuanduChariotResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> _targetFactory = targetFactory;
    public void CaptureGuanduChariotResponsePayments(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().CaptureGuanduChariotResponsePayments(action, parentFrameId, continuation);
    public global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default) => this._targetFactory().GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreCardActionsCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public bool IsHellDestroyedSoulMaterial(long useId, int cardId) => this._targetFactory().IsHellDestroyedSoulMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreCardActionsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsDiouSuspendedCompletedAttack(original, requiredWindowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed class CoreCardActionsCharacterZouShiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZouShiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeZouShiCapability { get => this._targetFactory().HasLeZouShiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreCardActionsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool UsesTreatedWounded { get => this._targetFactory().UsesTreatedWounded; }

    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreCardActionsCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseTargetIndex(long frameId, int targetIndex) => this._targetFactory().SetCardUseTargetIndex(frameId, targetIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCardActionsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime))]
internal sealed class CoreCardActionsCoreLastZoneConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreLastZoneConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _targetFactory = targetFactory;
    public void ValidateLastZoneConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse) => this._targetFactory().ValidateLastZoneConversionCommit(source, provider, materials, forResponse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuLuKangDixianCapability { get => this._targetFactory().HasWuLuKangDixianCapability; }

    public int? CaptureWuLuKangDixianDeclarationRank(int provider, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().CaptureWuLuKangDixianDeclarationRank(provider, ids);
    public bool IsWuLuKangKegouUse(long id) => this._targetFactory().IsWuLuKangKegouUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreCardActionsCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public bool TryBeginMouSmPrivateWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.MouSmPrivateCall call) => this._targetFactory().TryBeginMouSmPrivateWindow(attack, call);
    public bool IsMouSmPlayedWindow(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().IsMouSmPlayedWindow(action, window);
    public global::CardGame.Core.MouSmPlayedReturn? MouSmPlayedWindowReturn(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().MouSmPlayedWindowReturn(action, window);
    public bool TryReturnMouSmPrivateWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame p) => this._targetFactory().TryReturnMouSmPrivateWindow(p);
    public bool TryReturnMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryReturnMouSmPlayedWindow(window);
    public bool ValidMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidMouSmPlayedWindow(window);
    public bool IsMouSmPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsMouSmPlayedCostFinished(w, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CoreCardActionsCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public bool HasFinalizedTargetProgram(global::CardGame.Core.CardKind kind) => this._targetFactory().HasFinalizedTargetProgram(kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime))]
internal sealed class CoreCardActionsCharacterXinPiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterXinPiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _targetFactory = targetFactory;
    public bool HasXinPiCardObserver(long useId) => this._targetFactory().HasXinPiCardObserver(useId);
    public bool IsXinPiPhysicalClaim(long useId, int cardId) => this._targetFactory().IsXinPiPhysicalClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreCardActionsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool MatchesQinLangXianyingUseAction(long id, global::CardGame.Core.CardActionContext a, int cardId) => this._targetFactory().MatchesQinLangXianyingUseAction(id, a, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreCardActionsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsEphemeralCardDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreCardActionsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool MatchesMouZhijueUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesMouZhijueUseAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreCardActionsCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsAcceptedShouliMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool response) => this._targetFactory().IsAcceptedShouliMaterial(actor, card, source, response);
    public bool HasHengwuCapability { get => this._targetFactory().HasHengwuCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuLuXunZhangcaiCapability { get => this._targetFactory().HasWuLuXunZhangcaiCapability; }

    public int? CaptureWuLuXunResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().CaptureWuLuXunResponseRank(provider, effectiveKind, costs, source);
    public void CaptureWuLuXunZhangcai(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureWuLuXunZhangcai(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreCardActionsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsYouqueUse(long id) => this._targetFactory().IsYouqueUse(id);
    public bool IsXieweiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsXieweiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreCardActionsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueAcceptedCardResponse(attack, action, continuation);
    public void ContinueSlashAfterDodgePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashAfterDodgePrograms(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed class CoreCardActionsCharacterSunJianMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterSunJianMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouSunJianOlSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasMouSunJianOlSuspendedCompletedParent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed class CoreCardActionsCharacterZhouQunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhouQunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _targetFactory = targetFactory;
    public bool HasZhouQunCapability { get => this._targetFactory().HasZhouQunCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreCardActionsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesSelectedActorDuelAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesSelectedActorDuelAction(frameId, action, cardId);
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCardActionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreCardActionsCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool TracksDamageTargetBudgetTargets { get => this._targetFactory().TracksDamageTargetBudgetTargets; }

    public void SettleDamageMarkerBudgetTargets(long id) => this._targetFactory().SettleDamageMarkerBudgetTargets(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreCardActionsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttackAfterCardResolution(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => this._targetFactory().CompleteAttackAfterCardResolution(completion);
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreCardActionsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources) => this._targetFactory().ConsumeProgramViewAsUsage(sources);
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
    public bool TracksPlayCardHistory { get => this._targetFactory().TracksPlayCardHistory; }

    public global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card) => this._targetFactory().CaptureUsedCardSuit(actorSeat, physicalIds, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime))]
internal sealed class CoreCardActionsCharacterBaoSanNiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterBaoSanNiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> _targetFactory = targetFactory;
    public bool MatchesChosenAssaultUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesChosenAssaultUseAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreCardActionsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryCompleteKemingConverted(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => this._targetFactory().TryCompleteKemingConverted(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreCardActionsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime))]
internal sealed class CoreCardActionsCharacterQuHuangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterQuHuangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> _targetFactory = targetFactory;
    public bool HasQuHuangCapability { get => this._targetFactory().HasQuHuangCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreCardActionsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public bool TryMarkFinalizedTrickProgramsStarted(long frameId) => this._targetFactory().TryMarkFinalizedTrickProgramsStarted(frameId);
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueSimpleCardUse(frameId, continuation);
    public void ContinueCommittedTrickUse(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().ContinueCommittedTrickUse(frame);
    public bool TryMarkProgramUseAccepted(long frameId) => this._targetFactory().TryMarkProgramUseAccepted(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreCardActionsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime))]
internal sealed class CoreCardActionsCoreCompletedUndamagedTargetRevealObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCompletedUndamagedTargetRevealObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? CompletedUndamagedTargetRevealObserverRoot() => this._targetFactory().CompletedUndamagedTargetRevealObserverRoot();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardActionsCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreCardActionsCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public bool HasResponseUseCompletionObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasResponseUseCompletionObserver(action, continuation);
    public void ContinueCompletedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().ContinueCompletedResponseUse(attack, frame);
    public void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().ContinueNullificationAfterResponseUse(action);
    public bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind) => this._targetFactory().IsCompletedResponseUse(action, kind);
    public bool IsCompletedResponseParentConsistent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsCompletedResponseParentConsistent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreCardActionsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreCardActionsCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool IsHuanpeiUse(long id) => this._targetFactory().IsHuanpeiUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreCardActionsCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool IsWeijieUse(long id) => this._targetFactory().IsWeijieUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed class CoreCardActionsCharacterWoLongFengChuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterWoLongFengChuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _targetFactory = targetFactory;
    public bool HasWoLongFengChuUseObserver(long id) => this._targetFactory().HasWoLongFengChuUseObserver(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime))]
internal sealed class CoreCardActionsCharacterLiuZanMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiuZanMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> _targetFactory = targetFactory;
    public bool TracksAlternateActualUseColor { get => this._targetFactory().TracksAlternateActualUseColor; }

    public void RecordAlternateActualUseColor(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordAlternateActualUseColor(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreCardActionsCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime))]
internal sealed class CoreCardActionsCharacterTianChouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterTianChouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> _targetFactory = targetFactory;
    public bool MatchesTianChouShandaoUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => this._targetFactory().MatchesTianChouShandaoUseAction(id, action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreCardActionsCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool IsSayingRemovedMaterial(long useId, int cardId) => this._targetFactory().IsSayingRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed class CoreCardActionsCoreCardUseDebitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreCardUseDebitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseDebitIdentity? GetCardUseDebit(long actionId) => this._targetFactory().GetCardUseDebit(actionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreCardActionsCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public void CompleteQigongNativePayment(long id) => this._targetFactory().CompleteQigongNativePayment(id);
    public bool HasLvKuangLvXiangCardObserver(long originalUseId) => this._targetFactory().HasLvKuangLvXiangCardObserver(originalUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime))]
internal sealed class CoreCardActionsCoreSoleTargetProtectionObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSoleTargetProtectionObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> _targetFactory = targetFactory;
    public bool HasSoleTargetProtectionCardObserver(long useId) => this._targetFactory().HasSoleTargetProtectionCardObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreCardActionsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasLiuPanCapability { get => this._targetFactory().HasLiuPanCapability; }

    public global::CardGame.Core.ProgramSkillWindowContext CreatePijingIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreatePijingIssuedContext(frame, candidate, context);
    public bool TryCompletePijingZeroTargetUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryCompletePijingZeroTargetUse(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreCardActionsCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void PopFinishedCardUse(long frameId) => this._targetFactory().PopFinishedCardUse(frameId);
    public void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteFinishedAttackCardUse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreCardActionsCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CaoChunHandUsePayment? CaoChunPaidUseQualification(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaoChunPaidUseQualification(action);
    public global::CardGame.Core.ProgramTriggerCandidate? CaoChunDebtCandidate(global::CardGame.Core.CaoChunHandUsePayment p) => this._targetFactory().CaoChunDebtCandidate(p);
    public global::CardGame.Core.ProgramSkillWindowContext CaoChunDebtContext(global::CardGame.Core.CaoChunHandUsePayment p, long windowId) => this._targetFactory().CaoChunDebtContext(p, windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreCardActionsCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public bool HasFeiYiHezhong { get => this._targetFactory().HasFeiYiHezhong; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime))]
internal sealed class CoreCardActionsCharacterZhuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterZhuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnZhuJunLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().TryReturnZhuJunLegacyAlcohol(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreCardActionsCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool HasAdjacentRedTargetCapability { get => this._targetFactory().HasAdjacentRedTargetCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime))]
internal sealed class CoreCardActionsCharacterLuZhiMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterLuZhiMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouLzSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().HasMouLzSuspendedCompletedParent(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime))]
internal sealed class CoreCardActionsCoreKenshangPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreKenshangPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsKenshangRemovedMaterial(long useId, int cardId) => this._targetFactory().IsKenshangRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreCardActionsCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public bool TryCompleteSoleTargetQuarantinedUse(long id) => this._targetFactory().TryCompleteSoleTargetQuarantinedUse(id);
    public bool IsSoleTargetQuarantinedMaterial(long id, int cardId) => this._targetFactory().IsSoleTargetQuarantinedMaterial(id, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed class CoreCardActionsCharacterHuangfuSongWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuangfuSongWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuHuangFuSongCapability { get => this._targetFactory().HasWuHuangFuSongCapability; }

    public global::CardGame.Core.CardActionContext CaptureWuHfUseAction(global::CardGame.Core.CardActionContext a) => this._targetFactory().CaptureWuHfUseAction(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreCardActionsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public void CaptureLeCaoZhiWenlanQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureLeCaoZhiWenlanQualification(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime))]
internal sealed class CoreCardActionsCharacterGuanYuHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterGuanYuHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyGuanYuHeartSlashCapability { get => this._targetFactory().HasHappyGuanYuHeartSlashCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed class CoreCardActionsCharacterHuCheErAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterHuCheErAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId) => this._targetFactory().IsAnniversaryHuCheErClaimedUseMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreCardActionsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime))]
internal sealed class CoreCardActionsCharacterQiaoRuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCharacterQiaoRuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasQiaoRuiCompletedUseObserver(long parentUseId) => this._targetFactory().HasQiaoRuiCompletedUseObserver(parentUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreCardActionsCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId) => this._targetFactory().FindOwnedPlayableCard(actor, cardId);
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime))]
internal sealed class CoreCardActionsCoreNativeRecastInvocationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardActionsCoreNativeRecastInvocationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = default, int? instructionIndex = default, int? participantCursor = default, global::CardGame.Core.CardMoveReason? reason = default) => this._targetFactory().CaptureNativeRecastInvocation(initiatorSeat, executorSeat, originalPrimaryIds, parentFrameId, instructionIndex, participantCursor, reason);
    public void CompleteNativeRecastInvocation(global::CardGame.Core.NativeRecastInvocationDraft? draft, int actualDrawCount) => this._targetFactory().CompleteNativeRecastInvocation(draft, actualDrawCount);
}
