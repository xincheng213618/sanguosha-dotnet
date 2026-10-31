#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuEntries
{
    internal void AnswerSourceHuanfu(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceHuanfu(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSourceExecutionerHuanfu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSourceExecutionerHuanfu(supplied);
    internal global::CardGame.Core.PromptChoice ChooseSourceHuanfuAi(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseSourceHuanfuAi(d, f);
    internal void ContinueSourceHuanfu(long id) => _resolve().ContinueSourceHuanfu(id);
    internal global::CardGame.Core.CardUseFrame ExactSourceHuanfuParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactSourceHuanfuParent(f);
    internal void FinishSourceHuanfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSourceHuanfu(f);
    internal bool ResumeSourceHuanfu(long id) => _resolve().ResumeSourceHuanfu(id);
    internal void ReturnSourceHuanfuDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnSourceHuanfuDraw(supplied, outcome);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleSourceHuanfu(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => _resolve().SettleSourceHuanfu(f, use);
    internal bool SourceHuanfuArmQualifies(int owner, string binding, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().SourceHuanfuArmQualifies(owner, binding, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceHuanfuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceHuanfuChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceHuanfuCosts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceHuanfuCosts(f);
    internal global::CardGame.Core.CardUseFrame? SourceHuanfuUse(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().SourceHuanfuUse(context);
}
