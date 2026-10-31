#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingOwnedCardSelectionsEntries
{
    internal bool CanRunDyingOwnedCard(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunDyingOwnedCard(candidate, trigger, context);
    internal IReadOnlyList<PromptChoice> DyingOwnedCardChoices(ProgramSkillFrame root, SkillProgramEffect effect, int victim) => _resolve().DyingOwnedCardChoices(root, effect, victim);
    internal bool ExactDyingOwnedCardEntry(ProgramSkillFrame root, scoped out DyingFrame dying, scoped out ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactDyingOwnedCardEntry(root, out dying, out entry);
    internal void ResolveDyingOwnedCardChoice(PromptChoice choice) => _resolve().ResolveDyingOwnedCardChoice(choice);
    internal PromptChoice SelectAiDyingOwnedCard(PendingDecision decision) => _resolve().SelectAiDyingOwnedCard(decision);
    internal SkillProgramStepOutcome SelectDyingOwnedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectDyingOwnedCard(frame, effect);
}
