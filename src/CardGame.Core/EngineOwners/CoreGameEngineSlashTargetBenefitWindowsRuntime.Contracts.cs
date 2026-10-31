#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSlashTargetBenefitWindowsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreSlashTargetBenefitWindowsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSlashTargetBenefitWindowsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSameTypeAidLegacyProducer(global::CardGame.Core.CardUseFrame use, long producer);
    bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact);
}

internal interface ICoreSlashTargetBenefitWindowsCoreUniqueHpTargetAnnouncementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginUnannouncedUniqueHpTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy);
}

internal interface ICoreSlashTargetBenefitWindowsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreSlashTargetBenefitWindowsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSlashTargetBenefitWindowsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSlashTargetBenefitWindowsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardUseBeforeTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSlashTargetBenefitWindowsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreSlashTargetBenefitWindowsCoreSlashTargetBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidSlashTargetBenefitReceipt(global::CardGame.Core.SlashTargetBenefitReceipt r);
}

internal interface ICoreSlashTargetBenefitWindowsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashAfterDodgeCancellation(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSlashTargetBenefitWindowsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreSlashTargetBenefitWindowsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreSlashTargetBenefitWindowsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterFinalizedTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterProgramTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSlashTargetBenefitWindowsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreSlashTargetBenefitWindowsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreSlashTargetBenefitWindowsCoreSlashSuitDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashDodgeCancellationPrevented(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSlashTargetBenefitWindowsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSlashTargetBenefitWindowsCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ActualUseTargetReturnKind kind);
}

internal interface ICoreSlashTargetBenefitWindowsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreSlashTargetBenefitWindowsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreSlashTargetBenefitWindowsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreSlashTargetBenefitWindowsCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetPenalties(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}
