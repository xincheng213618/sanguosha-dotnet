#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuEntries
{
    internal void AnswerSourceHuanfu(PromptChoice selected) => _resolve().AnswerSourceHuanfu(selected);
    internal SkillProgramStepOutcome BeginSourceExecutionerHuanfu(ProgramSkillFrame supplied) => _resolve().BeginSourceExecutionerHuanfu(supplied);
    internal PromptChoice ChooseSourceHuanfuAi(PendingDecision d, ProgramSkillFrame f) => _resolve().ChooseSourceHuanfuAi(d, f);
    internal void ContinueSourceHuanfu(long id) => _resolve().ContinueSourceHuanfu(id);
    internal CardUseFrame ExactSourceHuanfuParent(ProgramSkillFrame f) => _resolve().ExactSourceHuanfuParent(f);
    internal void FinishSourceHuanfu(ProgramSkillFrame f) => _resolve().FinishSourceHuanfu(f);
    internal bool ResumeSourceHuanfu(long id) => _resolve().ResumeSourceHuanfu(id);
    internal void ReturnSourceHuanfuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnSourceHuanfuDraw(supplied, outcome);
    internal SkillProgramStepOutcome SettleSourceHuanfu(ProgramSkillFrame f, CardUseFrame use) => _resolve().SettleSourceHuanfu(f, use);
    internal bool SourceHuanfuArmQualifies(int owner, string binding, ProgramSkillWindowContext context) => _resolve().SourceHuanfuArmQualifies(owner, binding, context);
    internal IReadOnlyList<PromptChoice> SourceHuanfuChoices(ProgramSkillFrame f) => _resolve().SourceHuanfuChoices(f);
    internal IReadOnlyList<PromptChoice> SourceHuanfuCosts(ProgramSkillFrame f) => _resolve().SourceHuanfuCosts(f);
    internal CardUseFrame? SourceHuanfuUse(ProgramSkillWindowContext context) => _resolve().SourceHuanfuUse(context);
}
