#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceXiyingEntries
{
    internal void AnswerSourceXiying(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceXiying(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSourceXiying(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSourceXiying(supplied);
    internal void ContinueSourceXiying(long id) => _resolve().ContinueSourceXiying(id);
    internal bool DrainSourceXiying(long id) => _resolve().DrainSourceXiying(id);
    internal void FinishSourceXiying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSourceXiying(f);
    internal void PublishSourceXiying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSourceXiying(f);
    internal bool ResumeSourceXiying(long id) => _resolve().ResumeSourceXiying(id);
    internal global::CardGame.Core.PromptChoice SelectSourceXiyingAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectSourceXiyingAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceXiyingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceXiyingChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceXiyingPaymentChoices(global::CardGame.Core.ProgramSkillFrame f, int actor, bool initial) => _resolve().SourceXiyingPaymentChoices(f, actor, initial);
    internal bool SourceXiyingPlayParent(int owner, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().SourceXiyingPlayParent(owner, candidate, c);
    internal bool SourceXiyingMode { get => _resolve().SourceXiyingMode; }
}
