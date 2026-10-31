#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardBudgetAndFactionGiftsEntries
{
    internal int[] AvailableFactionGiftCosts(CardActionContext a) => _resolve().AvailableFactionGiftCosts(a);
    internal SkillProgramStepOutcome BeginCompletedFactionGift(ProgramSkillFrame f, string faction) => _resolve().BeginCompletedFactionGift(f, faction);
    internal SkillProgramStepOutcome BeginDiscardBudget(ProgramSkillFrame f) => _resolve().BeginDiscardBudget(f);
    internal SkillProgramStepOutcome BeginOutsideRangeDiscard(ProgramSkillFrame f) => _resolve().BeginOutsideRangeDiscard(f);
    internal Dictionary<string, string> BudgetGiftParameters(ProgramSkillFrame f, string action, string mode) => _resolve().BudgetGiftParameters(f, action, mode);
    internal bool BudgetGiftSourceValid(ProgramSkillFrame f) => _resolve().BudgetGiftSourceValid(f);
    internal IReadOnlyList<PromptChoice> CompletedFactionGiftChoices(ProgramSkillFrame f) => _resolve().CompletedFactionGiftChoices(f);
    internal string? CompletedFactionGiftFaction(ProgramSkillFrame f) => _resolve().CompletedFactionGiftFaction(f);
    internal bool CompletedFactionGiftLive(ProgramSkillFrame f, ProgramCompletedFactionGiftDraft d) => _resolve().CompletedFactionGiftLive(f, d);
    internal IReadOnlyList<PromptChoice> DiscardBudgetChoices(ProgramSkillFrame f) => _resolve().DiscardBudgetChoices(f);
    internal CardActionContext FactionGiftAction(ProgramSkillFrame f) => _resolve().FactionGiftAction(f);
    internal bool FactionGiftUsed(int provider) => _resolve().FactionGiftUsed(provider);
    internal IReadOnlyList<PromptChoice> OutsideRangeDiscardChoices(ProgramSkillFrame f) => _resolve().OutsideRangeDiscardChoices(f);
    internal int[] OutsideRangePlayers(int owner) => _resolve().OutsideRangePlayers(owner);
    internal SkillProgramStepOutcome PreventOwnPlayOutsideRange(ProgramSkillFrame f) => _resolve().PreventOwnPlayOutsideRange(f);
    internal void PublishBudgetGiftPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privacy = false) => _resolve().PublishBudgetGiftPrompt(f, chooser, text, choices, privacy);
    internal void ResolveCompletedFactionGift(PromptChoice c) => _resolve().ResolveCompletedFactionGift(c);
    internal void ResolveDiscardBudgetChoice(PromptChoice c) => _resolve().ResolveDiscardBudgetChoice(c);
    internal void ResolveOutsideRangeDiscard(PromptChoice c) => _resolve().ResolveOutsideRangeDiscard(c);
    internal bool ResumeBudgetGift(long id) => _resolve().ResumeBudgetGift(id);
}
