#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTopReorderEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramTopReorder(global::CardGame.Core.ProgramSkillFrame frame, int maximumCards, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, int? exactTopCount = default, global::CardGame.Core.ProgramPopulationThresholdCount? population = default, string? allBottomStateId = default, bool requireCurrentSeat = true) => _resolve().BeginProgramTopReorder(frame, maximumCards, numberExpression, exactTopCount, population, allBottomStateId, requireCurrentSeat);
    internal bool IsAiProgramTopReorderPending() => _resolve().IsAiProgramTopReorderPending();
    internal void PublishProgramTopReorderPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramTopReorderPrompt(frame);
    internal void ResolvePendingAiProgramTopReorder() => _resolve().ResolvePendingAiProgramTopReorder();
    internal void ResolveProgramTopReorderChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramTopReorderChoice(choice);
    internal global::CardGame.Core.CommandResult SubmitProgramTopReorderAnswer(global::CardGame.Core.PromptChoice choice) => _resolve().SubmitProgramTopReorderAnswer(choice);
}
