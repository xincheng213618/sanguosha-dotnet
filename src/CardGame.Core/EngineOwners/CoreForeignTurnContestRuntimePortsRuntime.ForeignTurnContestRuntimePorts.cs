#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreForeignTurnContestRuntimePortsRuntime
{
    Winner IForeignTurnContestFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IForeignTurnContestFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    bool IForeignTurnContestFacts.IsRuleWounded(int seat) => _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IForeignTurnContestFacts.HasSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool IForeignTurnContestContinuations.BeginRecoveryReplacement(long frameId) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement);
    bool IForeignTurnContestContinuations.BeginHpChanged(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    void IForeignTurnContestContinuations.GrantSelfOnly(ProgramSkillFrame frame, int subjectSeat) => _coreCardUseModulesOperations.GrantProgramTurnCardTargetRestriction(frame, SkillProgramCardTargetRestriction.SelfOnly, subjectSeat);
    void IForeignTurnContestContinuations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    bool IForeignTurnContestSlashProducer.CanIssue(int actorSeat, int targetSeat) => _coreForeignTurnContestsOperations.Value.CanUseForeignTurnContestSlash(actorSeat, targetSeat);
    ForeignTurnContestSlashIssue IForeignTurnContestSlashProducer.Capture(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin) => _coreForeignTurnContestsOperations.Value.CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    void IForeignTurnContestSlashProducer.Commit(ForeignTurnContestSlashIssue issue) => _coreForeignTurnContestsOperations.Value.CommitForeignTurnContestSlash(issue);
}
