#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDelayedPlacementTargetProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreDelayedPlacementTargetProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBianFuRenCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDiouCapability { get; }

    bool IsDiouUse(long id);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDinghanCapability { get; }

    bool HasDinghanCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHuangQuan { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterChenLinMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouChenLin { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryMarkProgramUseAccepted(long frameId);
    bool TryMarkFinalizedTrickProgramsStarted(long frameId);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGuoSiCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterFanChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFanChouCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalUseLightning(long id);
    bool BeginOriginalUseLightningFinalized(long id, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind actionKind);
    bool IsOriginalUseLightningFinalizedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    void ContinueOriginalUseLightningFinalized(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreDelayedPlacementTargetProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouXuHuangCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterSuFeiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSuFeiCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuYanCapability { get; }

    bool IsLiuYanLimuUse(long id);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterSimaShiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSimaShi { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCuiLieCapability { get; }

    bool HasAnniversaryCuiLieDelayedTail(global::CardGame.Core.CardUseFrame use);
    bool HasAnniversaryCuiLieCancellation(global::CardGame.Core.CardUseFrame use);
    bool TryBeginAnniversaryCuiLieDelayedPlacement(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreJianjiDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJianjiNativeUseCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterYuanTanYuanShangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNeifaCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCaoChunCapability { get; }
}

internal interface ICoreDelayedPlacementTargetProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteSoleTargetQuarantinedUse(long id);
}

internal interface ICoreDelayedPlacementTargetProgramsCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}
