#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingOwnedCardSelectionsEntries
{
    internal bool CanRunDyingOwnedCard(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunDyingOwnedCard(candidate, trigger, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DyingOwnedCardChoices(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.SkillProgramEffect effect, int victim) => _resolve().DyingOwnedCardChoices(root, effect, victim);
    internal bool ExactDyingOwnedCardEntry(global::CardGame.Core.ProgramSkillFrame root, scoped out global::CardGame.Core.DyingFrame dying, scoped out global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactDyingOwnedCardEntry(root, out dying, out entry);
    internal void ResolveDyingOwnedCardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDyingOwnedCardChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiDyingOwnedCard(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiDyingOwnedCard(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectDyingOwnedCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectDyingOwnedCard(frame, effect);
}
