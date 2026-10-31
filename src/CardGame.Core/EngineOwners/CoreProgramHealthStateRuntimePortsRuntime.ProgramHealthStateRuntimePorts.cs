#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramHealthStateRuntimePortsRuntime
{
    int IProgramHealthStateFacts.LivingFactionCount => _coreRuleQueriesOperations.GetLivingFactionCount();

    int IProgramHealthStateFacts.LivingPlayersMinHp => _coreRuleQueriesOperations.GetLivingPlayersMinHp();

    bool IProgramHealthStateFacts.HasCardPolicy(CharacterState player, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(player, policy);
    string IProgramHealthStateFacts.GetRuntimeSkillInstanceId(CharacterState player, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(player, skillId);
    bool IProgramHealthStateFacts.HasRuntimeSkillInstance(CharacterState player, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(player, skillId, instanceId);
    bool IProgramHealthStateFacts.HasWushuangTactic(int seat, int tactic) => _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, tactic);
    bool IProgramHealthStateFacts.HasQimeiHealthSubject(int seat) => _coreQimeiProgramsOperations.HasQimeiHealthSubject(seat);
    bool IProgramHealthStateFacts.HasMouSunJianOlHpSubject(int seat) => _characterSunJianMouOlOperations.HasMouSunJianOlHpSubject(seat);
    bool IProgramHealthStateFacts.HealthChangeSubject(int seat) => _programPipelineHooksOperations._programPipelineHooks.HealthChangeSubject(seat);
    bool IProgramHealthStateFacts.ShenwuCannotTurnOver(CharacterState player) => _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(player);
    bool IProgramHealthStateNativeEntries.TryHandleHappyQiaoRecoverTo(long frameId, int sourceSeat, int targetSeat, int targetHp) => _characterDaQiaoXiaoQiaoHappyGodOperations.TryHandleHappyQiaoRecoverTo(frameId, sourceSeat, targetSeat, targetHp);
    bool IProgramHealthStateNativeEntries.TryQueueRecoveryReplacement(long frameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion) => _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(frameId, sourceSeat, targetSeat, amount, completion);
    long IProgramHealthStateNativeEntries.BeginRecovery(long frameId, int sourceSeat, int targetSeat, int amount) => _coreNativeRecoveryFramesOperations.BeginRecovery(frameId, sourceSeat, targetSeat, amount);
    bool IProgramHealthStateNativeEntries.TryApplyHappyQiaoMaximumHp(long frameId, int targetSeat, int maximumHp, string sourceSkillId) => _characterDaQiaoXiaoQiaoHappyGodOperations.TryApplyHappyQiaoMaximumHp(frameId, targetSeat, maximumHp, sourceSkillId);
    void IProgramHealthStateNativeEntries.RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window) => _coreCharacterStateProgramsOperations.RecordCharacterStateChange(targetSeat, window);
    void IProgramHealthStateNativeEntries.RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => _coreCharacterTurnedOverProgramsOperations.RecordCharacterTurnedOver(targetSeat, wasFaceDown);
    void IProgramHealthStateNativeEntries.AddLog(string category, string message, int ownerSeat, int targetSeat) => _coreMatchLogOperations.AddLog(category, message, ownerSeat, targetSeat);
}
