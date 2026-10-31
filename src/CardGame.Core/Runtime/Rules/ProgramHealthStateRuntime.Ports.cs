namespace CardGame.Core;

internal interface IProgramHealthStateFacts : IProgramSelectionQuery
{
    int LivingFactionCount { get; }
    int LivingPlayersMinHp { get; }
    bool HasCardPolicy(CharacterState player, SkillProgramCardPolicyKind policy);
    string GetRuntimeSkillInstanceId(CharacterState player, string skillId);
    bool HasRuntimeSkillInstance(CharacterState player, string skillId, string instanceId);
    bool HasWushuangTactic(int seat, int tactic);
    bool HasQimeiHealthSubject(int seat);
    bool HasMouSunJianOlHpSubject(int seat);
    bool HealthChangeSubject(int seat);
    bool ShenwuCannotTurnOver(CharacterState player);
}

// These are native child protocols and event producers, not callbacks to a
// program mutation. Replacement/dual-health payment stays on its owning frame.
internal interface IProgramHealthStateNativeEntries : IProgramRuntimeOperations
{
    bool TryHandleHappyQiaoRecoverTo(long frameId, int sourceSeat, int targetSeat, int targetHp);
    bool TryQueueRecoveryReplacement(long frameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion);
    long BeginRecovery(long frameId, int sourceSeat, int targetSeat, int amount);
    bool TryApplyHappyQiaoMaximumHp(long frameId, int targetSeat, int maximumHp, string sourceSkillId);
    void RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window);
    void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown);
    void AddLog(string category, string message, int ownerSeat, int targetSeat);
}
