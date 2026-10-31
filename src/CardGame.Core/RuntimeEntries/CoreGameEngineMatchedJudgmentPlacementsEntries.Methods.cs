#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchedJudgmentPlacementsEntries
{
    internal SkillProgramStepOutcome BeginMatchedJudgmentPlacement(ProgramSkillFrame input, string bind, IReadOnlyList<Suit> suits) => _resolve().BeginMatchedJudgmentPlacement(input, bind, suits);
    internal IReadOnlyList<PromptChoice> MatchedJudgmentPlacementChoices(ProgramSkillFrame f) => _resolve().MatchedJudgmentPlacementChoices(f);
    internal void PublishMatchedJudgmentPlacement(ProgramSkillFrame f) => _resolve().PublishMatchedJudgmentPlacement(f);
    internal void ResolveMatchedJudgmentPlacement(PromptChoice choice) => _resolve().ResolveMatchedJudgmentPlacement(choice);
    internal bool ResumeMatchedJudgmentPlacement(long id) => _resolve().ResumeMatchedJudgmentPlacement(id);
    internal PromptChoice SelectAiMatchedJudgmentPlacement(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiMatchedJudgmentPlacement(decision, f);
}
