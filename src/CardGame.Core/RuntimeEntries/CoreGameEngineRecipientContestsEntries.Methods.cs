#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRecipientContest(global::CardGame.Core.ProgramSkillFrame supplied, int recipient, string bind) => _resolve().BeginRecipientContest(supplied, recipient, bind);
    internal bool CanStartRecipientContest(global::CardGame.Core.CharacterState owner) => _resolve().CanStartRecipientContest(owner);
    internal void CompletePindianWinnerSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompletePindianWinnerSlash(completion);
    internal void PublishRecipientContestThird(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRecipientContestThird(f);
    internal bool RecipientContestResultMatches(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.PindianResult result) => _resolve().RecipientContestResultMatches(f, out result);
    internal int[] RecipientContestThirds(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientContestThirds(f);
    internal void ResolveRecipientContestChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveRecipientContestChoice(choice);
    internal bool ResumeRecipientContest(long id) => _resolve().ResumeRecipientContest(id);
    internal bool ReturnRegisteredCompletePindianWinnerSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompletePindianWinnerSlash(completion);
    internal global::CardGame.Core.SkillProgramStepOutcome UseRecipientContestWinner(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().UseRecipientContestWinner(supplied, bind);
}
