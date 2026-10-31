#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramWindowRuntimeServicesCorePindianRevealWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompletePindianRevealBinding(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceDamageTriggerCandidate(global::CardGame.Core.DamageTriggerWindowFrame window);
}

internal interface ICoreProgramWindowRuntimeServicesCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPhaseHandDebtResolver(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreProgramWindowRuntimeServicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramWindowRuntimeServicesCharacterGuanYuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyGuanYuDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreProgramWindowRuntimeServicesCoreWushuangDyingExitOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteProgramJudgmentReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreProgramWindowRuntimeServicesCoreCardEffectProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteCardEffectCandidate(global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramWindowRuntimeServicesCharacterRuanHuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMingchaNormalDrawReplacement(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCoreSlashTargetBenefitWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteSlashTargetBenefitBinding(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBeiMiHuPreludeLifecycleWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramWindowRuntimeServicesCoreCompletedUseChildReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RestoreCompletedUseAttackAfterIssuedChild(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreProgramWindowRuntimeServicesCoreShanheYongzhanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheYongzhanDyingExitReturn(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramWindowRuntimeServicesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    void CompleteDyingProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreProgramWindowRuntimeServicesCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICoreProgramWindowRuntimeServicesCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteSlashTargetPenaltyBinding(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramWindowRuntimeServicesCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteForeignActualTurnStartBinding(global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreProgramWindowRuntimeServicesCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteActualUseTargetBinding(global::CardGame.Core.ProgramSkillFrame child, bool completed);
}
