#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentSlashFireOffersCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCurrentSlashFireOffersCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreCurrentSlashFireOffersCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCurrentSlashFireOffersCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor);
    bool HasGrantedPhaseEntityDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? ids);
}

internal interface ICoreCurrentSlashFireOffersCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinSpecificSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreCurrentSlashFireOffersCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
}

internal interface ICoreCurrentSlashFireOffersCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCurrentSlashFireOffersCoreSoleTargetRedirectOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetAfterProtectionRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetAfterProtectionRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCurrentSlashFireOffersCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreCurrentSlashFireOffersCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreCurrentSlashFireOffersCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCurrentSlashFireOffersResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentSlashFireOffersCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit);
}

internal interface ICoreCurrentSlashFireOffersCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPijingAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PijingTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets);
}

internal interface ICoreCurrentSlashFireOffersCoreShanheSourceLihuo3160Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ShanheSourceLihuoProspectiveTargetBonus(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCurrentSlashFireOffersCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreCurrentSlashFireOffersCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCurrentSlashFireOffersCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCurrentSlashFireOffersCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentSlashFireOffersCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreCurrentSlashFireOffersCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChenglieAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChenglieTargetsAppliedEvent e, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets);
}

internal interface ICoreCurrentSlashFireOffersCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuaManSouyingMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SouyingNativeMutationEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original);
}

internal interface ICoreCurrentSlashFireOffersCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreCurrentSlashFireOffersCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateCardTargetCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId = default);
}

internal interface ICoreCurrentSlashFireOffersCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact);
}

internal interface ICoreCurrentSlashFireOffersCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalFangtianHalberd { get; }
}

internal interface ICoreCurrentSlashFireOffersCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresSlashUseDistance(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreCurrentSlashFireOffersCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreCurrentSlashFireOffersCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedProvenanceUseDistance(long? useId, int owner);
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds);
}

internal interface ICoreCurrentSlashFireOffersCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreCurrentSlashFireOffersCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreCurrentSlashFireOffersCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreCurrentSlashFireOffersKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCurrentSlashFireOffersCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCurrentSlashFireOffersCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShortRangeSlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ShortRangeSlashTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
    bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCurrentSlashFireOffersCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreCurrentSlashFireOffersCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreCurrentSlashFireOffersCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashPolicyForColor(int actor, global::CardGame.Core.CardKind kind, bool? color);
}

internal interface ICoreCurrentSlashFireOffersCoreKuangfuReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedKuangfuDistance(long? id, int actor);
}

internal interface ICoreCurrentSlashFireOffersContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentSlashFireOffersCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}

internal interface ICoreCurrentSlashFireOffersCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}
