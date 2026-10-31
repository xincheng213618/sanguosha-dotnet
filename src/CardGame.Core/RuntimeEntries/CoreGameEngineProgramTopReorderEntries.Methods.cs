#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTopReorderEntries
{
    internal SkillProgramStepOutcome BeginProgramTopReorder(ProgramSkillFrame frame, int maximumCards, SkillProgramNumberExpression? numberExpression, int? exactTopCount = default, ProgramPopulationThresholdCount? population = default, string? allBottomStateId = default, bool requireCurrentSeat = true) => _resolve().BeginProgramTopReorder(frame, maximumCards, numberExpression, exactTopCount, population, allBottomStateId, requireCurrentSeat);
    internal bool IsAiProgramTopReorderPending() => _resolve().IsAiProgramTopReorderPending();
    internal void PublishProgramTopReorderPrompt(ProgramSkillFrame frame) => _resolve().PublishProgramTopReorderPrompt(frame);
    internal void ResolvePendingAiProgramTopReorder() => _resolve().ResolvePendingAiProgramTopReorder();
    internal void ResolveProgramTopReorderChoice(PromptChoice choice) => _resolve().ResolveProgramTopReorderChoice(choice);
    internal CommandResult SubmitProgramTopReorderAnswer(PromptChoice choice) => _resolve().SubmitProgramTopReorderAnswer(choice);
}
