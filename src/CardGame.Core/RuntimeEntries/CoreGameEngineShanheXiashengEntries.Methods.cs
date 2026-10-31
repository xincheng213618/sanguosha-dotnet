#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiashengEntries
{
    internal PromptChoice AiShanheXiasheng(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().AiShanheXiasheng(prompt, frame);
    internal void AnswerShanheXiasheng(PromptChoice selected) => _resolve().AnswerShanheXiasheng(selected);
    internal SkillProgramStepOutcome BeginShanheXiasheng(ProgramSkillFrame supplied, string stage) => _resolve().BeginShanheXiasheng(supplied, stage);
    internal bool CanOfferShanheXiasheng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheXiasheng(candidate, trigger, context);
    internal bool IsShanheXiashengTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> originalTargets) => _resolve().IsShanheXiashengTargetFact(use, fact, originalTargets);
    internal bool ResumeShanheXiasheng(long frameId) => _resolve().ResumeShanheXiasheng(frameId);
    internal void ReturnShanheXiashengDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheXiashengDraw(supplied, outcome);
    internal CardActionContext? ShanheXiashengAction(int owner, ProgramSkillWindowContext context) => _resolve().ShanheXiashengAction(owner, context);
    internal (int Red, int Black) ShanheXiashengColors(int seat) => _resolve().ShanheXiashengColors(seat);
    internal IReadOnlyList<IReadOnlyList<int>> ShanheXiashengExtraTargets(CardUseFrame use) => _resolve().ShanheXiashengExtraTargets(use);
    internal bool ShanheXiashengRequiresTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ShanheXiashengRequiresTargetWindow(query);
}
