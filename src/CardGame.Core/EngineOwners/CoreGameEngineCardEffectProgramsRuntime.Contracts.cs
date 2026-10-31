#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardEffectProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreCardEffectProgramsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreCardEffectProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreCardEffectProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCardEffectProgramsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? BeginJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, global::CardGame.Core.CardKind? sourceCard, global::CardGame.Core.JudgmentContinuationKind continuation, global::CardGame.Core.Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility? programResultVisibility = default, global::CardGame.Core.OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default);
}

internal interface ICoreCardEffectProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardEffectProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardEffectProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreCardEffectProgramsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginOriginalUseLightningJudgment(global::CardGame.Core.DelayedEffectContinuation delayed);
}

internal interface ICoreCardEffectProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreCardEffectProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardEffectProgramsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat);
}

internal interface ICoreCardEffectProgramsCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPerTargetTrickEffectBinding(global::CardGame.Core.ProgramCardTriggerCandidate c);
    void ReturnNativeTrickTargetEffect(global::CardGame.Core.CardEffectBeforeApplyFrame window);
    bool ValidNativeTrickTargetEffect(global::CardGame.Core.CardEffectBeforeApplyFrame window);
}

internal interface ICoreCardEffectProgramsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDelayedJudgmentOrTurnStart(global::CardGame.Core.CharacterState current, int? excludedCardId = default);
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
}

internal interface ICoreCardEffectProgramsCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.NativeSlashTargetDuelPlan plan, out global::CardGame.Core.NativeSlashTargetDuelSlice slice);
    global::CardGame.Core.CardActionContext NativeSlashTargetDuelEffectAction(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeSlashTargetDuelSlice slice);
}

internal interface ICoreCardEffectProgramsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveNullifiableEffectCore(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreCardEffectProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeFrame(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}
