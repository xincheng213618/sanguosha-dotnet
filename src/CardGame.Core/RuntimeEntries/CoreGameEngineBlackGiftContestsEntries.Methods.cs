#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackGiftContestsEntries
{
    internal bool AllowsGiveBlackHandAndResolveRecipientContestActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsGiveBlackHandAndResolveRecipientContestActivation(gate);
    internal SkillProgramStepOutcome BeginBlackGiftContest(ProgramSkillFrame supplied, int recipient) => _resolve().BeginBlackGiftContest(supplied, recipient);
    internal IReadOnlyList<PromptChoice> BlackGiftChoices(ProgramSkillFrame f) => _resolve().BlackGiftChoices(f);
    internal IReadOnlyList<PromptChoice> BlackGiftDiscardOptions(ProgramSkillFrame f) => _resolve().BlackGiftDiscardOptions(f);
    internal bool BlackGiftMaterialLegal(int owner, Card card) => _resolve().BlackGiftMaterialLegal(owner, card);
    internal bool BlackGiftResultMatches(ProgramSkillFrame f, scoped out PindianResult result) => _resolve().BlackGiftResultMatches(f, out result);
    internal int[] BlackGiftSecondSeats(ProgramSkillFrame f) => _resolve().BlackGiftSecondSeats(f);
    internal bool DrainRecipientConsequences(ProgramSkillFrame f, bool movement) => _resolve().DrainRecipientConsequences(f, movement);
    internal void PublishBlackGiftChoice(ProgramSkillFrame f) => _resolve().PublishBlackGiftChoice(f);
    internal void ResolveBlackGiftChoice(PromptChoice choice) => _resolve().ResolveBlackGiftChoice(choice);
    internal bool ResumeBlackGiftContest(long id) => _resolve().ResumeBlackGiftContest(id);
}
