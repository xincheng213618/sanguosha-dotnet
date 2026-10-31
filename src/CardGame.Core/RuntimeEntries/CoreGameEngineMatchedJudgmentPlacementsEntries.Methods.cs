#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchedJudgmentPlacementsEntries
{
    internal void AssertMatchedJudgmentPlacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMatchedJudgmentPlacement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMatchedJudgmentPlacement(global::CardGame.Core.ProgramSkillFrame input, string bind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits) => _resolve().BeginMatchedJudgmentPlacement(input, bind, suits);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MatchedJudgmentPlacementChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchedJudgmentPlacementChoices(f);
    internal void PublishMatchedJudgmentPlacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMatchedJudgmentPlacement(f);
    internal void ResolveMatchedJudgmentPlacement(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveMatchedJudgmentPlacement(choice);
    internal bool ResumeMatchedJudgmentPlacement(long id) => _resolve().ResumeMatchedJudgmentPlacement(id);
    internal global::CardGame.Core.PromptChoice SelectAiMatchedJudgmentPlacement(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMatchedJudgmentPlacement(decision, f);
}
