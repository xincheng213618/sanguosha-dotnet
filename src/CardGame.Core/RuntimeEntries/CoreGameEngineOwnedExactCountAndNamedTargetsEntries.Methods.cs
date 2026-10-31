#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedExactCountAndNamedTargetsEntries
{
    internal void AssertProgramNamedBoundTargetSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertProgramNamedBoundTargetSelection(frame, paused);
    internal bool CanStartExactOwnedCardCount(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().CanStartExactOwnedCardCount(owner, plan);
    internal void FreezeExactOwnedSelectionSuits(long frameId, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => _resolve().FreezeExactOwnedSelectionSuits(frameId, effect, cardIds, locations);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NamedBoundTargetChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramNamedBoundTargetSelection draft) => _resolve().NamedBoundTargetChoices(frame, draft);
    internal void ResolveProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramNamedBoundTargets(frame, effect, selected);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectProgramNamedBoundTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectProgramNamedBoundTargets(frame, effect);
}
