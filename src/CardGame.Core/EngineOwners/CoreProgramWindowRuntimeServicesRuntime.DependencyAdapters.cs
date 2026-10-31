#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCorePindianRevealWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCorePindianRevealWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _targetFactory = targetFactory;
    public void CompletePindianRevealBinding(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CompletePindianRevealBinding(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreNativeDamageWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreNativeDamageWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _targetFactory = targetFactory;
    public void AdvanceDamageTriggerCandidate(global::CardGame.Core.DamageTriggerWindowFrame window) => this._targetFactory().AdvanceDamageTriggerCandidate(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public bool IsPhaseHandDebtResolver(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().IsPhaseHandDebtResolver(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCharacterGuanYuHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCharacterGuanYuHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyGuanYuDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsHappyGuanYuDyingExitReturn(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreWushuangDyingExitAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreWushuangDyingExitOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> _targetFactory = targetFactory;
    public bool IsWushuangDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsWushuangDyingExitReturn(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteProgramJudgmentReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().CompleteProgramJudgmentReplacementBinding(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreCardEffectProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreCardEffectProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteCardEffectCandidate(global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteCardEffectCandidate(context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCharacterRuanHuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCharacterRuanHuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime> _targetFactory = targetFactory;
    public bool IsMingchaNormalDrawReplacement(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsMingchaNormalDrawReplacement(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreSlashTargetBenefitWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreSlashTargetBenefitWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> _targetFactory = targetFactory;
    public void CompleteSlashTargetBenefitBinding(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CompleteSlashTargetBenefitBinding(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool ValidBeiMiHuPreludeLifecycleWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().ValidBeiMiHuPreludeLifecycleWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreCompletedUseChildReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreCompletedUseChildReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> _targetFactory = targetFactory;
    public void RestoreCompletedUseAttackAfterIssuedChild(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().RestoreCompletedUseAttackAfterIssuedChild(program, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreShanheYongzhanAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreShanheYongzhanOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime> _targetFactory = targetFactory;
    public bool IsShanheYongzhanDyingExitReturn(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsShanheYongzhanDyingExitReturn(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public void CompleteDyingProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().CompleteDyingProgramBinding(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCharacterYanPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCharacterYanPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> _targetFactory = targetFactory;
    public bool IsYanPuDyingExitEdge(int index) => this._targetFactory().IsYanPuDyingExitEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreSlashTargetPenaltyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreSlashTargetPenaltyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> _targetFactory = targetFactory;
    public void CompleteSlashTargetPenaltyBinding(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CompleteSlashTargetPenaltyBinding(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public void CompleteForeignActualTurnStartBinding(global::CardGame.Core.ProgramSkillFrame child) => this._targetFactory().CompleteForeignActualTurnStartBinding(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime))]
internal sealed class CoreProgramWindowRuntimeServicesCoreActualUseTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowRuntimeServicesCoreActualUseTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteActualUseTargetBinding(global::CardGame.Core.ProgramSkillFrame child, bool completed) => this._targetFactory().CompleteActualUseTargetBinding(child, completed);
}
