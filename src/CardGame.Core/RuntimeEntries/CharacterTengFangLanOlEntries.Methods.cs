#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTengFangLanOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AichenProgramRemoveOption(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().AichenProgramRemoveOption(frame, effect);
    internal global::System.Collections.Generic.HashSet<string> AichenRemovedOptions(int ownerSeat) => _resolve().AichenRemovedOptions(ownerSeat);
    internal bool CanRunLuochongResolve(int ownerSeat) => _resolve().CanRunLuochongResolve(ownerSeat);
    internal global::System.Collections.Generic.IReadOnlyList<(string OptionId, int TargetSeat)> CollectLuochongAvailablePairs(int ownerSeat, scoped out (global::System.Collections.Generic.IReadOnlySet<string> Options, global::System.Collections.Generic.IReadOnlySet<int> Targets) ledger) => _resolve().CollectLuochongAvailablePairs(ownerSeat, out ledger);
    internal void CompleteLuochongDiscardStage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteLuochongDiscardStage(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome FinalizeLuochongResolution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.LuochongPendingState pending) => _resolve().FinalizeLuochongResolution(frame, pending);
    internal bool IsLuochongOptionAvailableFor(int ownerSeat, string optionId, int targetSeat, global::System.Collections.Generic.IReadOnlySet<string> usedOptions, global::System.Collections.Generic.IReadOnlySet<int> usedTargets, global::System.Collections.Generic.IReadOnlySet<string> removedOptions) => _resolve().IsLuochongOptionAvailableFor(ownerSeat, optionId, targetSeat, usedOptions, usedTargets, removedOptions);
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)> LuochongDiscardPool(int targetSeat) => _resolve().LuochongDiscardPool(targetSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome LuochongProgramResolve(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().LuochongProgramResolve(frame, effect);
    internal global::CardGame.Core.ProgramLuochongResolvedEvent LuochongResolvedEvent(global::CardGame.Core.ProgramSkillFrame active, global::CardGame.Core.LuochongPendingState pending) => _resolve().LuochongResolvedEvent(active, pending);
    internal (global::System.Collections.Generic.IReadOnlySet<string> Options, global::System.Collections.Generic.IReadOnlySet<int> Targets) LuochongRoundLedger(int ownerSeat) => _resolve().LuochongRoundLedger(ownerSeat);
    internal void PresentLuochongDiscardPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentLuochongDiscardPrompt(frame);
    internal void ResolveAichenRemoveChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveAichenRemoveChoice(selected);
    internal void ResolveLuochongDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveLuochongDiscardChoice(selected);
    internal void ResolveLuochongPickChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveLuochongPickChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiLuochongResolveChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiLuochongResolveChoice(decision);
}
