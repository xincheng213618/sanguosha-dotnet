#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreForeignTurnContestRuntimePortsRuntime
{
    Winner global::CardGame.Core.IForeignTurnContestFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IForeignTurnContestFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    bool global::CardGame.Core.IForeignTurnContestFacts.IsRuleWounded(int seat) => _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IForeignTurnContestFacts.HasSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool global::CardGame.Core.IForeignTurnContestFacts.HasAllowedSlashTargetTail(CardUseFrame use) => ((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || _characterMaWanOlOperations.HasMaWanHunjiangTargetTail(use) || (_characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(use) || _characterMaXiuMaTieOlOperations.HasKenshangTargetTail(use))) || _coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || _coreShortRangeSlashTargetsOperations.HasShortRangeSlashTail(use) || _coreOriginalTargetAdditionsOperations.HasIssuedOriginalTargetAdditionTail(use) || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use);
    bool global::CardGame.Core.IForeignTurnContestContinuations.BeginRecoveryReplacement(long frameId) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement);
    bool global::CardGame.Core.IForeignTurnContestContinuations.BeginHpChanged(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    void global::CardGame.Core.IForeignTurnContestContinuations.GrantSelfOnly(ProgramSkillFrame frame, int subjectSeat) => _coreCardUseModulesOperations.GrantProgramTurnCardTargetRestriction(frame, SkillProgramCardTargetRestriction.SelfOnly, subjectSeat);
    void global::CardGame.Core.IForeignTurnContestContinuations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    bool global::CardGame.Core.IForeignTurnContestSlashProducer.CanIssue(int actorSeat, int targetSeat) => _coreForeignTurnContestsOperations.CanUseForeignTurnContestSlash(actorSeat, targetSeat);
    ForeignTurnContestSlashIssue global::CardGame.Core.IForeignTurnContestSlashProducer.Capture(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin) => _coreForeignTurnContestsOperations.CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    void global::CardGame.Core.IForeignTurnContestSlashProducer.Commit(ForeignTurnContestSlashIssue issue) => _coreForeignTurnContestsOperations.CommitForeignTurnContestSlash(issue);
}
