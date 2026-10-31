#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardBudgetAndFactionGiftsEntries
{
    internal int[] AvailableFactionGiftCosts(global::CardGame.Core.CardActionContext a) => _resolve().AvailableFactionGiftCosts(a);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCompletedFactionGift(global::CardGame.Core.ProgramSkillFrame f, string faction) => _resolve().BeginCompletedFactionGift(f, faction);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDiscardBudget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginDiscardBudget(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOutsideRangeDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginOutsideRangeDiscard(f);
    internal global::System.Collections.Generic.Dictionary<string, string> BudgetGiftParameters(global::CardGame.Core.ProgramSkillFrame f, string action, string mode) => _resolve().BudgetGiftParameters(f, action, mode);
    internal bool BudgetGiftSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BudgetGiftSourceValid(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CompletedFactionGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletedFactionGiftChoices(f);
    internal string? CompletedFactionGiftFaction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletedFactionGiftFaction(f);
    internal bool CompletedFactionGiftLive(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramCompletedFactionGiftDraft d) => _resolve().CompletedFactionGiftLive(f, d);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DiscardBudgetChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DiscardBudgetChoices(f);
    internal global::CardGame.Core.CardActionContext FactionGiftAction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FactionGiftAction(f);
    internal bool FactionGiftUsed(int provider) => _resolve().FactionGiftUsed(provider);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OutsideRangeDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OutsideRangeDiscardChoices(f);
    internal int[] OutsideRangePlayers(int owner) => _resolve().OutsideRangePlayers(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome PreventOwnPlayOutsideRange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PreventOwnPlayOutsideRange(f);
    internal void PublishBudgetGiftPrompt(global::CardGame.Core.ProgramSkillFrame f, int chooser, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, bool privacy = false) => _resolve().PublishBudgetGiftPrompt(f, chooser, text, choices, privacy);
    internal void ResolveCompletedFactionGift(global::CardGame.Core.PromptChoice c) => _resolve().ResolveCompletedFactionGift(c);
    internal void ResolveDiscardBudgetChoice(global::CardGame.Core.PromptChoice c) => _resolve().ResolveDiscardBudgetChoice(c);
    internal void ResolveOutsideRangeDiscard(global::CardGame.Core.PromptChoice c) => _resolve().ResolveOutsideRangeDiscard(c);
    internal bool ResumeBudgetGift(long id) => _resolve().ResumeBudgetGift(id);
}
