#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestsEntries
{
    internal SkillProgramStepOutcome BeginRecipientContest(ProgramSkillFrame supplied, int recipient, string bind) => _resolve().BeginRecipientContest(supplied, recipient, bind);
    internal bool CanStartRecipientContest(CharacterState owner) => _resolve().CanStartRecipientContest(owner);
    internal void CompletePindianWinnerSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().CompletePindianWinnerSlash(completion);
    internal void PublishRecipientContestThird(ProgramSkillFrame f) => _resolve().PublishRecipientContestThird(f);
    internal bool RecipientContestResultMatches(ProgramSkillFrame f, scoped out PindianResult result) => _resolve().RecipientContestResultMatches(f, out result);
    internal int[] RecipientContestThirds(ProgramSkillFrame f) => _resolve().RecipientContestThirds(f);
    internal void ResolveRecipientContestChoice(PromptChoice choice) => _resolve().ResolveRecipientContestChoice(choice);
    internal bool ResumeRecipientContest(long id) => _resolve().ResumeRecipientContest(id);
    internal bool ReturnRegisteredCompletePindianWinnerSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompletePindianWinnerSlash(completion);
    internal SkillProgramStepOutcome UseRecipientContestWinner(ProgramSkillFrame supplied, string bind) => _resolve().UseRecipientContestWinner(supplied, bind);
}
