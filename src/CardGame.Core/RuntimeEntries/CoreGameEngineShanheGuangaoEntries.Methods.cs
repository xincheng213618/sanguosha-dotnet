#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuangaoEntries
{
    internal SkillProgramStepOutcome BeginShanheGuangao(ProgramSkillFrame supplied) => _resolve().BeginShanheGuangao(supplied);
    internal bool CanOfferShanheGuangao(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheGuangao(candidate, trigger, context);
    internal void FinishShanheGuangao(ProgramSkillFrame frame) => _resolve().FinishShanheGuangao(frame);
    internal void PublishShanheGuangaoPrompt(ProgramSkillFrame frame) => _resolve().PublishShanheGuangaoPrompt(frame);
    internal CardUseFrame RequireShanheGuangaoUse(ProgramSkillFrame frame) => _resolve().RequireShanheGuangaoUse(frame);
    internal void ResolveShanheGuangaoChoice(PromptChoice selected) => _resolve().ResolveShanheGuangaoChoice(selected);
    internal bool ResumeShanheGuangao(long frameId) => _resolve().ResumeShanheGuangao(frameId);
    internal void ReturnShanheGuangaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheGuangaoDraw(supplied, outcome);
    internal PromptChoice SelectAiShanheGuangao(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiShanheGuangao(decision, frame);
    internal int[] ShanheGuangaoCandidates(int owner, CardUseFrame use) => _resolve().ShanheGuangaoCandidates(owner, use);
    internal IReadOnlyList<PromptChoice> ShanheGuangaoChoices(ProgramSkillFrame frame) => _resolve().ShanheGuangaoChoices(frame);
}
