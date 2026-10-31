#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientConsequencesReceiptsEntries
{
    internal bool BlackGiftGameIssued(int owner, string skill) => _resolve().BlackGiftGameIssued(owner, skill);
    internal bool CanRunRecipientConsequences(ProgramTriggerCandidate c, SkillProgramTrigger trigger) => _resolve().CanRunRecipientConsequences(c, trigger);
    internal bool PrintedLordGameIssued(int owner, string skill, string binding) => _resolve().PrintedLordGameIssued(owner, skill, binding);
    internal bool RecipientConsequencesGameEnded() => _resolve().RecipientConsequencesGameEnded();
    internal bool ReturnRecipientConsequencesMovement(ProgramSkillFrame f) => _resolve().ReturnRecipientConsequencesMovement(f);
    internal bool ValidBlackGiftContest(ProgramSkillFrame f) => _resolve().ValidBlackGiftContest(f);
    internal bool ValidPrintedLordBenefit(ProgramSkillFrame f) => _resolve().ValidPrintedLordBenefit(f);
}
