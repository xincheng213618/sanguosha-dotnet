#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientConsequencesReceiptsEntries
{
    internal bool BlackGiftGameIssued(int owner, string skill) => _resolve().BlackGiftGameIssued(owner, skill);
    internal bool CanRunRecipientConsequences(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanRunRecipientConsequences(c, trigger);
    internal bool IsRecipientConsequencesMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsRecipientConsequencesMovement(f, e, pending);
    internal bool PrintedLordGameIssued(int owner, string skill, string binding) => _resolve().PrintedLordGameIssued(owner, skill, binding);
    internal bool RecipientConsequencesGameEnded() => _resolve().RecipientConsequencesGameEnded();
    internal bool ReturnRecipientConsequencesMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRecipientConsequencesMovement(f);
    internal bool ValidBlackGiftContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBlackGiftContest(f);
    internal bool ValidPrintedLordBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPrintedLordBenefit(f);
}
