#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool TryGetWeifuVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary) => this._targetFactory().TryGetWeifuVirtualPrimaryReturn(use, parent, out primary);
    public bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasWeifuTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasChenglieTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeDragonBoatAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeDragonBoatAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreRecipientCategorySlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreRecipientCategorySlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> _targetFactory = targetFactory;
    public bool HasRecipientCategorySlashTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasRecipientCategorySlashTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, global::CardGame.Core.SkillProgramTriggerFacts facts) => this._targetFactory().CreateCardActionProgramContext(action, window, parentFrameId, ownerSeat, eventTargetSeat, facts);
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreProgramCardUseRolesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreProgramCardUseRolesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CloneRoleAction(global::CardGame.Core.CardActionContext action, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().CloneRoleAction(action, actorSeat, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreOriginalTargetAdditionsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeGuanduAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeGuanduAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CanUseSlashTarget(source, target, slashCard, conversionSource, effectiveKind, ignoreDistance, existingUseFrameId, noEffectiveRank, specificEffectiveRank, physicalCardIds, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame EnhancementCardUse(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().EnhancementCardUse(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreTieredRoundActualTargetWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreTieredRoundActualTargetWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> _targetFactory = targetFactory;
    public void SyncIssuedTieredRoundZeroTrickTargetWindows(long useId, global::CardGame.Core.CardActionContext previousAction) => this._targetFactory().SyncIssuedTieredRoundZeroTrickTargetWindows(useId, previousAction);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterMaWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterMaWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> _targetFactory = targetFactory;
    public bool TryGetMaWanVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary) => this._targetFactory().TryGetMaWanVirtualPrimaryReturn(use, parent, out primary);
    public bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasMaWanHunjiangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreSameTypeActualUseAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreSameTypeActualUseAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasSameTypeAidTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreForeignContestAidObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreForeignContestAidObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _targetFactory = targetFactory;
    public bool TryGetSameTypeAidVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary) => this._targetFactory().TryGetSameTypeAidVirtualPrimaryReturn(use, parent, out primary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public bool CanBeWenheAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => this._targetFactory().CanBeWenheAdditionalTarget(use, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool TryGetSouyingCancelledVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget) => this._targetFactory().TryGetSouyingCancelledVirtualPrimaryReturn(use, parent, out primaryTarget);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureProgramTriggerFacts(owner, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => this._targetFactory().FieldEquipmentSlot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateProgramTurnEffectGrant(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreDesignatedExtraTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreDesignatedExtraTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _targetFactory = targetFactory;
    public bool TryGetDesignatedExtraTargetVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryGetDesignatedExtraTargetVirtualPrimaryReturn(use, parent, out targets);
    public bool HasDesignatedExtraTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasDesignatedExtraTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasPijingTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCurrentEnhancementVirtualReturnAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCurrentEnhancementVirtualReturnOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementVirtualReturnRuntime> _targetFactory = targetFactory;
    public bool TryGetCurrentEnhancementVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget) => this._targetFactory().TryGetCurrentEnhancementVirtualPrimaryReturn(use, parent, out primaryTarget);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public bool TryGetCurrentSlashFirePrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary) => this._targetFactory().TryGetCurrentSlashFirePrimaryReturn(use, parent, out primary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreShortRangeSlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreShortRangeSlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _targetFactory = targetFactory;
    public bool TryGetShortRangeVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> target) => this._targetFactory().TryGetShortRangeVirtualPrimaryReturn(use, parent, out target);
    public bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasShortRangeSlashTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool TryGetZhangYanVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary) => this._targetFactory().TryGetZhangYanVirtualPrimaryReturn(use, parent, out primary);
    public bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasZhangYanLangdaoTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasBingzhongTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreOriginalTargetAdditionsCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasKenshangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreOriginalTargetAdditionsCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalTargetAdditionsCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}
