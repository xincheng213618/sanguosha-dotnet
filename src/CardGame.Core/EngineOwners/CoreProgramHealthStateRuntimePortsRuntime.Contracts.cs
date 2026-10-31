#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramHealthStateRuntimePortsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetLivingFactionCount();
    int GetLivingPlayersMinHp();
}

internal interface ICoreProgramHealthStateRuntimePortsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreCharacterTurnedOverProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreProgramHealthStateRuntimePortsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryHandleHappyQiaoRecoverTo(long frameId, int ownerSeat, int targetSeat, int value);
    bool TryApplyHappyQiaoMaximumHp(long parentFrameId, int targetSeat, int newMaximum, string sourceId);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreQimeiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQimeiHealthSubject(int seat);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangTactic(int seat, int id);
}

internal interface ICoreProgramHealthStateRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramHealthStateRuntimePortsCoreShenwuNpcProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenwuCannotTurnOver(global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramHealthStateRuntimePortsCoreNativeRecoveryFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount);
}

internal interface ICoreProgramHealthStateRuntimePortsCharacterSunJianMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSunJianOlHpSubject(int seat);
}
