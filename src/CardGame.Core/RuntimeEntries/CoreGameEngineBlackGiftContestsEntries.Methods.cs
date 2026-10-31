#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackGiftContestsEntries
{
    internal bool AllowsGiveBlackHandAndResolveRecipientContestActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsGiveBlackHandAndResolveRecipientContestActivation(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBlackGiftContest(global::CardGame.Core.ProgramSkillFrame supplied, int recipient) => _resolve().BeginBlackGiftContest(supplied, recipient);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BlackGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BlackGiftChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BlackGiftDiscardOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BlackGiftDiscardOptions(f);
    internal bool BlackGiftMaterialLegal(int owner, global::CardGame.Core.Card card) => _resolve().BlackGiftMaterialLegal(owner, card);
    internal bool BlackGiftResultMatches(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.PindianResult result) => _resolve().BlackGiftResultMatches(f, out result);
    internal int[] BlackGiftSecondSeats(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BlackGiftSecondSeats(f);
    internal bool DrainRecipientConsequences(global::CardGame.Core.ProgramSkillFrame f, bool movement) => _resolve().DrainRecipientConsequences(f, movement);
    internal void PublishBlackGiftChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBlackGiftChoice(f);
    internal void ResolveBlackGiftChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveBlackGiftChoice(choice);
    internal bool ResumeBlackGiftContest(long id) => _resolve().ResumeBlackGiftContest(id);
}
