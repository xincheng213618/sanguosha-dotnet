#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentTurnSkillSuppressionsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long CaptureGuanduQualificationStamp(long underlying);
}

internal interface ICoreCurrentTurnSkillSuppressionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDyingWindowGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    long CaptureDyingWindowQualificationStamp(long underlying);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentTurnSkillSuppressionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreNativeFactionChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long CaptureNativeFactionQualificationStamp(long underlying);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterXiahouJieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryXiahouJieGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    long CaptureAnniversaryXiahouJieQualificationStamp(long underlying);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileDuoruiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreWushuangOriginalSkillRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangYashiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreShenwuExpandedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenwuFengdongQualifiesGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    long CaptureShenwuFengdongStamp();
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterZouShiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool YunzhengNonLockedGrantQualified(global::CardGame.Core.CharacterState holder, global::CardGame.Core.SkillGrant grant);
    long CaptureYunzhengQualificationStamp(long inner);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnOwnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryCuiLieGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    long CaptureAnniversaryCuiLieQualificationStamp(long underlying);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreAnqiFeirenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnqiFeirenGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterZhangRenMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiZrFuanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYinzhiSkillSuppressed(int seat);
}

internal interface ICoreCurrentTurnSkillSuppressionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterMaChaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileMaChaoTingweiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLishiPhaseSuppressed(int seat);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterCaoPiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouCaoPiSkillsQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreCurrentTurnSkillSuppressionsCharacterWangGuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMiuyanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    long CaptureMiuyanQualificationStamp(long underlying);
}
