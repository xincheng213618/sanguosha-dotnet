#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramHealthStateRuntimePortsRuntime
{
    int global::CardGame.Core.IProgramHealthStateFacts.LivingFactionCount => _coreRuleQueriesOperations.GetLivingFactionCount();

    int global::CardGame.Core.IProgramHealthStateFacts.LivingPlayersMinHp => _coreRuleQueriesOperations.GetLivingPlayersMinHp();

    bool global::CardGame.Core.IProgramHealthStateFacts.HasCardPolicy(CharacterState player, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(player, policy);
    string global::CardGame.Core.IProgramHealthStateFacts.GetRuntimeSkillInstanceId(CharacterState player, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(player, skillId);
    bool global::CardGame.Core.IProgramHealthStateFacts.HasRuntimeSkillInstance(CharacterState player, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(player, skillId, instanceId);
    bool global::CardGame.Core.IProgramHealthStateFacts.HasWushuangTactic(int seat, int tactic) => _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, tactic);
    bool global::CardGame.Core.IProgramHealthStateFacts.HasQimeiHealthSubject(int seat) => _coreQimeiProgramsOperations.HasQimeiHealthSubject(seat);
    bool global::CardGame.Core.IProgramHealthStateFacts.HasMouSunJianOlHpSubject(int seat) => _characterSunJianMouOlOperations.HasMouSunJianOlHpSubject(seat);
    bool global::CardGame.Core.IProgramHealthStateFacts.HealthChangeSubject(int seat) => _programPipelineHooksOperations._programPipelineHooks.HealthChangeSubject(seat);
    bool global::CardGame.Core.IProgramHealthStateFacts.ShenwuCannotTurnOver(CharacterState player) => _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(player);
    bool global::CardGame.Core.IProgramHealthStateNativeEntries.TryHandleHappyQiaoRecoverTo(long frameId, int sourceSeat, int targetSeat, int targetHp) => _characterDaQiaoXiaoQiaoHappyGodOperations.TryHandleHappyQiaoRecoverTo(frameId, sourceSeat, targetSeat, targetHp);
    bool global::CardGame.Core.IProgramHealthStateNativeEntries.TryQueueRecoveryReplacement(long frameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion) => _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(frameId, sourceSeat, targetSeat, amount, completion);
    long global::CardGame.Core.IProgramHealthStateNativeEntries.BeginRecovery(long frameId, int sourceSeat, int targetSeat, int amount) => _coreNativeRecoveryFramesOperations.BeginRecovery(frameId, sourceSeat, targetSeat, amount);
    bool global::CardGame.Core.IProgramHealthStateNativeEntries.TryApplyHappyQiaoMaximumHp(long frameId, int targetSeat, int maximumHp, string sourceSkillId) => _characterDaQiaoXiaoQiaoHappyGodOperations.TryApplyHappyQiaoMaximumHp(frameId, targetSeat, maximumHp, sourceSkillId);
    void global::CardGame.Core.IProgramHealthStateNativeEntries.RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window) => _coreCharacterStateProgramsOperations.RecordCharacterStateChange(targetSeat, window);
    void global::CardGame.Core.IProgramHealthStateNativeEntries.RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => _coreCharacterTurnedOverProgramsOperations.RecordCharacterTurnedOver(targetSeat, wasFaceDown);
    void global::CardGame.Core.IProgramHealthStateNativeEntries.AddLog(string category, string message, int ownerSeat, int targetSeat) => _coreMatchLogOperations.AddLog(category, message, ownerSeat, targetSeat);
}
