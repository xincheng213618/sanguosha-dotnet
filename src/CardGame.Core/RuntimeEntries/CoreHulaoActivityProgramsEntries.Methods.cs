#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHulaoActivityProgramsEntries
{
    internal bool ContinuePaidHulaoAfterSkillLoss(ProgramSkillFrame f) => _resolve().ContinuePaidHulaoAfterSkillLoss(f);
    internal SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteHulaoActivity(supplied, effect);
    internal bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().HulaoActivityFrameRidesOn(child, parent);
    internal bool HulaoAwaitMovementChildren(long id) => _resolve().HulaoAwaitMovementChildren(id);
    internal IReadOnlyList<PromptChoice> HulaoPeachChoices(ProgramSkillFrame f) => _resolve().HulaoPeachChoices(f);
    internal IReadOnlyList<Card> HulaoPeaches(ProgramSkillFrame f) => _resolve().HulaoPeaches(f);
    internal bool HulaoProgramSourceValid(ProgramSkillFrame f) => _resolve().HulaoProgramSourceValid(f);
    internal IReadOnlyList<PromptChoice> HulaoXiuluoChoices(ProgramSkillFrame f) => _resolve().HulaoXiuluoChoices(f);
    internal IReadOnlyList<PromptChoice> HulaoXiuluoJudgmentChoices(ProgramSkillFrame f) => _resolve().HulaoXiuluoJudgmentChoices(f);
    internal void PublishHulaoPeach(ProgramSkillFrame f) => _resolve().PublishHulaoPeach(f);
    internal void PublishHulaoPrompt(ProgramSkillFrame f, string text, IReadOnlyList<PromptChoice> choices) => _resolve().PublishHulaoPrompt(f, text, choices);
    internal bool PublishHulaoXiuluo(ProgramSkillFrame f) => _resolve().PublishHulaoXiuluo(f);
    internal bool PublishHulaoXiuluoJudgment(ProgramSkillFrame f) => _resolve().PublishHulaoXiuluoJudgment(f);
    internal void ResolveHulaoPeachChoice(PromptChoice answer) => _resolve().ResolveHulaoPeachChoice(answer);
    internal void ResolveHulaoXiuluoChoice(PromptChoice answer) => _resolve().ResolveHulaoXiuluoChoice(answer);
    internal void ResolveHulaoXiuluoJudgmentChoice(PromptChoice answer) => _resolve().ResolveHulaoXiuluoJudgmentChoice(answer);
    internal bool ResumeHulaoActivity(long id) => _resolve().ResumeHulaoActivity(id);
    internal void ReturnHulaoPeachUse(CardUseFrame use) => _resolve().ReturnHulaoPeachUse(use);
    internal PromptChoice SelectAiHulaoPeach(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiHulaoPeach(decision, f);
    internal PromptChoice SelectAiHulaoXiuluo(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiHulaoXiuluo(decision, f);
    internal PromptChoice SelectHulaoActivityChoice(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectHulaoActivityChoice(decision, f);
    internal void ValidateHulaoPeachUse(CardUseFrame use) => _resolve().ValidateHulaoPeachUse(use);
}
