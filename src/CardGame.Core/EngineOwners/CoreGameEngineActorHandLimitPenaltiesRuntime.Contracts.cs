#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActorHandLimitPenaltiesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreActorHandLimitPenaltiesCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedZeroEntityDuel(long id);
}

internal interface ICoreActorHandLimitPenaltiesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActorHandLimitPenaltiesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreActorHandLimitPenaltiesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActorHandLimitPenaltiesCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreActorHandLimitPenaltiesCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreActorHandLimitPenaltiesCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChainedStateBasicUse(long id);
}

internal interface ICoreActorHandLimitPenaltiesCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreActorHandLimitPenaltiesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreActorHandLimitPenaltiesCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreActorHandLimitPenaltiesCoreForeignPublicPileSlashesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignPublicPileSlashUse(long resolutionId);
}

internal interface ICoreActorHandLimitPenaltiesCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActorHandLimitPenaltiesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreActorHandLimitPenaltiesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCoreKenshangPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangRemovedMaterial(long useId, int cardId);
}

internal interface ICoreActorHandLimitPenaltiesCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTieredRoundZeroUse(long id);
}
