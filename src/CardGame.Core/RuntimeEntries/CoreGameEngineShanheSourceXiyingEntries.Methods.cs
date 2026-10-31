#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceXiyingEntries
{
    internal void AnswerSourceXiying(PromptChoice selected) => _resolve().AnswerSourceXiying(selected);
    internal SkillProgramStepOutcome BeginSourceXiying(ProgramSkillFrame supplied) => _resolve().BeginSourceXiying(supplied);
    internal void ContinueSourceXiying(long id) => _resolve().ContinueSourceXiying(id);
    internal bool DrainSourceXiying(long id) => _resolve().DrainSourceXiying(id);
    internal void FinishSourceXiying(ProgramSkillFrame f) => _resolve().FinishSourceXiying(f);
    internal void PublishSourceXiying(ProgramSkillFrame f) => _resolve().PublishSourceXiying(f);
    internal bool ResumeSourceXiying(long id) => _resolve().ResumeSourceXiying(id);
    internal PromptChoice SelectSourceXiyingAi(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectSourceXiyingAi(p, f);
    internal IReadOnlyList<PromptChoice> SourceXiyingChoices(ProgramSkillFrame f) => _resolve().SourceXiyingChoices(f);
    internal IReadOnlyList<PromptChoice> SourceXiyingPaymentChoices(ProgramSkillFrame f, int actor, bool initial) => _resolve().SourceXiyingPaymentChoices(f, actor, initial);
    internal bool SourceXiyingPlayParent(int owner, ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().SourceXiyingPlayParent(owner, candidate, c);
    internal bool SourceXiyingMode { get => _resolve().SourceXiyingMode; }
}
