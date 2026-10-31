#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreShortRangeSlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreShortRangeSlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _targetFactory = targetFactory;
    public bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasShortRangeSlashTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterMaWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterMaWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> _targetFactory = targetFactory;
    public bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasMaWanHunjiangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreSameTypeActualUseAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreSameTypeActualUseAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasSameTypeAidTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasBingzhongTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsCurrentSlashFireChangedUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasZhangYanLangdaoTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreOriginalTargetAdditionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreOriginalTargetAdditionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _targetFactory = targetFactory;
    public bool HasIssuedOriginalTargetAdditionTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasIssuedOriginalTargetAdditionTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasKenshangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasWeifuTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void GrantProgramTurnCardTargetRestriction(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int targetSeat) => this._targetFactory().GrantProgramTurnCardTargetRestriction(frame, restriction, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasChenglieTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public bool CanUseForeignTurnContestSlash(int actor, int target) => this._targetFactory().CanUseForeignTurnContestSlash(actor, target);
    public global::CardGame.Core.ForeignTurnContestSlashIssue CaptureForeignTurnContestSlash(long programFrameId, int instructionIndex, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.ForeignTurnPindianOrigin origin) => this._targetFactory().CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    public void CommitForeignTurnContestSlash(global::CardGame.Core.ForeignTurnContestSlashIssue issue) => this._targetFactory().CommitForeignTurnContestSlash(issue);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreForeignTurnContestRuntimePortsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestRuntimePortsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasPijingTargetTail(use);
}
