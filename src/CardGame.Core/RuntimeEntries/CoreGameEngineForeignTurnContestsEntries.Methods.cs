#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignTurnContestsEntries
{
    internal void AdvanceForeignActualTurnStartCandidate(global::CardGame.Core.ForeignActualTurnStartWindowFrame parent, bool activated, bool completed) => _resolve().AdvanceForeignActualTurnStartCandidate(parent, activated, completed);
    internal void AssertForeignTurnContest(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertForeignTurnContest(frame);
    internal bool CanUseForeignTurnContestSlash(int actor, int target) => _resolve().CanUseForeignTurnContestSlash(actor, target);
    internal global::CardGame.Core.ForeignTurnContestSlashIssue CaptureForeignTurnContestSlash(long programFrameId, int instructionIndex, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.ForeignTurnPindianOrigin origin) => _resolve().CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    internal void CommitForeignTurnContestSlash(global::CardGame.Core.ForeignTurnContestSlashIssue issue) => _resolve().CommitForeignTurnContestSlash(issue);
    internal void CompleteForeignActualTurnStartBinding(global::CardGame.Core.ProgramSkillFrame child) => _resolve().CompleteForeignActualTurnStartBinding(child);
    internal global::CardGame.Core.ForeignTurnPindianOrigin? FreezeForeignTurnPindianOrigin(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.PindianFrame pindian) => _resolve().FreezeForeignTurnPindianOrigin(program, pindian);
    internal bool IsForeignActualTurnPindian(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsForeignActualTurnPindian(frame);
    internal bool ReturnRegisteredCompleteForeignTurnContestSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteForeignTurnContestSlash(completion);
    internal bool TryBeginForeignActualTurnStart(global::CardGame.Core.CharacterState turnOwner) => _resolve().TryBeginForeignActualTurnStart(turnOwner);
}
