#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreJudgmentOutcomeDamageEdgesCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreJudgmentOutcomeDamageEdgesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreJudgmentOutcomeDamageEdgesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreDamagePreventionProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeforeDamageHasNoSource(global::CardGame.Core.BeforeDamageProgramWindowFrame frame);
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreProgramJudgmentOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? JudgmentOutcomeInstruction(global::CardGame.Core.ProgramSkillFrame frame, int instructionIndex);
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreZhenyiJudgmentWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactZhenyiBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreJudgmentOutcomeDamageEdgesCoreDamageJudgmentSuitPaymentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDamageJudgmentHpLossDyingRide(global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramSkillFrame program);
}
