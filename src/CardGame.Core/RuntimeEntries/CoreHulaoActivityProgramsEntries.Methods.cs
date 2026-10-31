#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHulaoActivityProgramsEntries
{
    internal bool ContinuePaidHulaoAfterSkillLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHulaoAfterSkillLoss(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteHulaoActivity(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteHulaoActivity(supplied, effect);
    internal bool HulaoActivityFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().HulaoActivityFrameRidesOn(child, parent);
    internal bool HulaoAwaitMovementChildren(long id) => _resolve().HulaoAwaitMovementChildren(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HulaoPeachChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HulaoPeachChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> HulaoPeaches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HulaoPeaches(f);
    internal bool HulaoProgramSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HulaoProgramSourceValid(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HulaoXiuluoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HulaoXiuluoChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HulaoXiuluoJudgmentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HulaoXiuluoJudgmentChoices(f);
    internal void PublishHulaoPeach(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHulaoPeach(f);
    internal void PublishHulaoPrompt(global::CardGame.Core.ProgramSkillFrame f, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishHulaoPrompt(f, text, choices);
    internal bool PublishHulaoXiuluo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHulaoXiuluo(f);
    internal bool PublishHulaoXiuluoJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHulaoXiuluoJudgment(f);
    internal void ResolveHulaoPeachChoice(global::CardGame.Core.PromptChoice answer) => _resolve().ResolveHulaoPeachChoice(answer);
    internal void ResolveHulaoXiuluoChoice(global::CardGame.Core.PromptChoice answer) => _resolve().ResolveHulaoXiuluoChoice(answer);
    internal void ResolveHulaoXiuluoJudgmentChoice(global::CardGame.Core.PromptChoice answer) => _resolve().ResolveHulaoXiuluoJudgmentChoice(answer);
    internal bool ResumeHulaoActivity(long id) => _resolve().ResumeHulaoActivity(id);
    internal void ReturnHulaoPeachUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnHulaoPeachUse(use);
    internal global::CardGame.Core.PromptChoice SelectAiHulaoPeach(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiHulaoPeach(decision, f);
    internal global::CardGame.Core.PromptChoice SelectAiHulaoXiuluo(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiHulaoXiuluo(decision, f);
    internal global::CardGame.Core.PromptChoice SelectHulaoActivityChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectHulaoActivityChoice(decision, f);
    internal void ValidateHulaoPeachUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateHulaoPeachUse(use);
}
