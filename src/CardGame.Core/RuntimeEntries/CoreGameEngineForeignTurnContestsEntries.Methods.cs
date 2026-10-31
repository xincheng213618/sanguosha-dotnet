#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignTurnContestsEntries
{
    internal void AdvanceForeignActualTurnStartCandidate(ForeignActualTurnStartWindowFrame parent, bool activated, bool completed) => _resolve().AdvanceForeignActualTurnStartCandidate(parent, activated, completed);
    internal void AssertForeignTurnContest(ProgramSkillFrame frame) => _resolve().AssertForeignTurnContest(frame);
    internal bool CanUseForeignTurnContestSlash(int actor, int target) => _resolve().CanUseForeignTurnContestSlash(actor, target);
    internal ForeignTurnContestSlashIssue CaptureForeignTurnContestSlash(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin) => _resolve().CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    internal void CommitForeignTurnContestSlash(ForeignTurnContestSlashIssue issue) => _resolve().CommitForeignTurnContestSlash(issue);
    internal void CompleteForeignActualTurnStartBinding(ProgramSkillFrame child) => _resolve().CompleteForeignActualTurnStartBinding(child);
    internal ForeignTurnPindianOrigin? FreezeForeignTurnPindianOrigin(ProgramSkillFrame program, PindianFrame pindian) => _resolve().FreezeForeignTurnPindianOrigin(program, pindian);
    internal bool IsForeignActualTurnPindian(ProgramSkillFrame frame) => _resolve().IsForeignActualTurnPindian(frame);
    internal bool ReturnRegisteredCompleteForeignTurnContestSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteForeignTurnContestSlash(completion);
    internal bool TryBeginForeignActualTurnStart(CharacterState turnOwner) => _resolve().TryBeginForeignActualTurnStart(turnOwner);
}
