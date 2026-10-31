#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreForeignTurnContestRuntimePortsCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterMaWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedOriginalTargetAdditionTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramTurnCardTargetRestriction(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int targetSeat);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreForeignTurnContestRuntimePortsCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseForeignTurnContestSlash(int actor, int target);
    global::CardGame.Core.ForeignTurnContestSlashIssue CaptureForeignTurnContestSlash(long programFrameId, int instructionIndex, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.ForeignTurnPindianOrigin origin);
    void CommitForeignTurnContestSlash(global::CardGame.Core.ForeignTurnContestSlashIssue issue);
}

internal interface ICoreForeignTurnContestRuntimePortsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use);
}
