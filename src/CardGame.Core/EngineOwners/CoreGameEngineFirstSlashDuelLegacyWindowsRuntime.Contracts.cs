#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFirstSlashDuelLegacyWindowsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ActualUseTargetReturnKind kind);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreFirstSlashDuelLegacyWindowsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOngoingActualUseTurn { get; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetPenalties(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreSlashTargetBenefitWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreFirstSlashDuelBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFirstSlashDuelBenefitCapability { get; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreFirstSlashDuelLegacyWindowsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}
