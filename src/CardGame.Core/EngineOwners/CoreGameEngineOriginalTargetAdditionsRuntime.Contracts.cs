#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOriginalTargetAdditionsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetWeifuVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary);
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreOriginalTargetAdditionsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeDragonBoatAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreOriginalTargetAdditionsCoreRecipientCategorySlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRecipientCategorySlashTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, global::CardGame.Core.SkillProgramTriggerFacts facts);
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
}

internal interface ICoreOriginalTargetAdditionsCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CloneRoleAction(global::CardGame.Core.CardActionContext action, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreOriginalTargetAdditionsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreOriginalTargetAdditionsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreOriginalTargetAdditionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOriginalTargetAdditionsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeGuanduAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreOriginalTargetAdditionsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreOriginalTargetAdditionsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreOriginalTargetAdditionsCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame EnhancementCardUse(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreOriginalTargetAdditionsCoreTieredRoundActualTargetWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SyncIssuedTieredRoundZeroTrickTargetWindows(long useId, global::CardGame.Core.CardActionContext previousAction);
}

internal interface ICoreOriginalTargetAdditionsCharacterMaWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetMaWanVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary);
    bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreForeignContestAidObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetSameTypeAidVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary);
}

internal interface ICoreOriginalTargetAdditionsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeWenheAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreOriginalTargetAdditionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreOriginalTargetAdditionsCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetSouyingCancelledVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget);
}

internal interface ICoreOriginalTargetAdditionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreOriginalTargetAdditionsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreOriginalTargetAdditionsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreOriginalTargetAdditionsCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreOriginalTargetAdditionsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreOriginalTargetAdditionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreOriginalTargetAdditionsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreOriginalTargetAdditionsCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetDesignatedExtraTargetVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> targets);
    bool HasDesignatedExtraTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreCurrentEnhancementVirtualReturnOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetCurrentEnhancementVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget);
}

internal interface ICoreOriginalTargetAdditionsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreOriginalTargetAdditionsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetCurrentSlashFirePrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary);
}

internal interface ICoreOriginalTargetAdditionsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreOriginalTargetAdditionsCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetShortRangeVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> target);
    bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetZhangYanVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primary);
    bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreOriginalTargetAdditionsCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOriginalTargetAdditionsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreOriginalTargetAdditionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
