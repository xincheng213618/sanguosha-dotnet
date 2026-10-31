#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePaidOwnTargetObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICorePaidOwnTargetObserversKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICorePaidOwnTargetObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePaidOwnTargetObserversCoreForeignContestAidObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetObserver(long useId);
}

internal interface ICorePaidOwnTargetObserversCoreRecipientContestObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUniqueHpPeerActualTargetObserver(long useId);
}

internal interface ICorePaidOwnTargetObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidOwnTargetObserversCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICorePaidOwnTargetObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICorePaidOwnTargetObserversCoreEarnedActualEndingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueEarnedActualEnding(global::CardGame.Core.ProgramSkillFrame frame, string skillId, string bindingId);
}

internal interface ICorePaidOwnTargetObserversCoreOwnTrickDrawAlcoholObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OwnTrickDrawLegacyAlcoholRide(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent, int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidOwnTargetObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICorePaidOwnTargetObserversCoreLostHpOwnedGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome RunLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICorePaidOwnTargetObserversCoreOwnTrickDrawTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OwnTrickDrawMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent);
    bool OwnTrickDrawFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICorePaidOwnTargetObserversKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICorePaidOwnTargetObserversCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesActualUseTarget(global::CardGame.Core.ActualUseTargetIdentity identity);
    bool CanRunActualUseTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICorePaidOwnTargetObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICorePaidOwnTargetObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidOwnTargetObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICorePaidOwnTargetObserversCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICorePaidOwnTargetObserversCoreHalfHandPhaseDebtObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandSupportObserverRootMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent);
}

internal interface ICorePaidOwnTargetObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePaidOwnTargetObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
