#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTengFangLanOlEntries
{
    internal SkillProgramStepOutcome AichenProgramRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().AichenProgramRemoveOption(frame, effect);
    internal HashSet<string> AichenRemovedOptions(int ownerSeat) => _resolve().AichenRemovedOptions(ownerSeat);
    internal bool CanRunLuochongResolve(int ownerSeat) => _resolve().CanRunLuochongResolve(ownerSeat);
    internal IReadOnlyList<(string OptionId, int TargetSeat)> CollectLuochongAvailablePairs(int ownerSeat, scoped out (IReadOnlySet<string> Options, IReadOnlySet<int> Targets) ledger) => _resolve().CollectLuochongAvailablePairs(ownerSeat, out ledger);
    internal void CompleteLuochongDiscardStage(ProgramSkillFrame frame) => _resolve().CompleteLuochongDiscardStage(frame);
    internal SkillProgramStepOutcome FinalizeLuochongResolution(ProgramSkillFrame frame, LuochongPendingState pending) => _resolve().FinalizeLuochongResolution(frame, pending);
    internal bool IsLuochongOptionAvailableFor(int ownerSeat, string optionId, int targetSeat, IReadOnlySet<string> usedOptions, IReadOnlySet<int> usedTargets, IReadOnlySet<string> removedOptions) => _resolve().IsLuochongOptionAvailableFor(ownerSeat, optionId, targetSeat, usedOptions, usedTargets, removedOptions);
    internal IReadOnlyList<(Card Card, CardLocation Location)> LuochongDiscardPool(int targetSeat) => _resolve().LuochongDiscardPool(targetSeat);
    internal SkillProgramStepOutcome LuochongProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().LuochongProgramResolve(frame, effect);
    internal ProgramLuochongResolvedEvent LuochongResolvedEvent(ProgramSkillFrame active, LuochongPendingState pending) => _resolve().LuochongResolvedEvent(active, pending);
    internal (IReadOnlySet<string> Options, IReadOnlySet<int> Targets) LuochongRoundLedger(int ownerSeat) => _resolve().LuochongRoundLedger(ownerSeat);
    internal void PresentLuochongDiscardPrompt(ProgramSkillFrame frame) => _resolve().PresentLuochongDiscardPrompt(frame);
    internal void ResolveAichenRemoveChoice(PromptChoice selected) => _resolve().ResolveAichenRemoveChoice(selected);
    internal void ResolveLuochongDiscardChoice(PromptChoice selected) => _resolve().ResolveLuochongDiscardChoice(selected);
    internal void ResolveLuochongPickChoice(PromptChoice selected) => _resolve().ResolveLuochongPickChoice(selected);
    internal PromptChoice SelectAiLuochongResolveChoice(PendingDecision decision) => _resolve().SelectAiLuochongResolveChoice(decision);
}
