#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedExactCountAndNamedTargetsEntries
{
    internal void AssertProgramNamedBoundTargetSelection(ProgramSkillFrame frame, SkillProgramEffect paused) => _resolve().AssertProgramNamedBoundTargetSelection(frame, paused);
    internal bool CanStartExactOwnedCardCount(CharacterState owner, ProgramExecutionPlan plan) => _resolve().CanStartExactOwnedCardCount(owner, plan);
    internal void FreezeExactOwnedSelectionSuits(long frameId, SkillProgramEffect effect, IReadOnlyList<int> cardIds, IReadOnlyList<CardLocation> locations) => _resolve().FreezeExactOwnedSelectionSuits(frameId, effect, cardIds, locations);
    internal IReadOnlyList<PromptChoice> NamedBoundTargetChoices(ProgramSkillFrame frame, ProgramNamedBoundTargetSelection draft) => _resolve().NamedBoundTargetChoices(frame, draft);
    internal void ResolveProgramNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _resolve().ResolveProgramNamedBoundTargets(frame, effect, selected);
    internal SkillProgramStepOutcome SelectProgramNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectProgramNamedBoundTargets(frame, effect);
}
