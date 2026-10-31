#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuangaoEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheGuangao(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheGuangao(supplied);
    internal bool CanOfferShanheGuangao(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheGuangao(candidate, trigger, context);
    internal void FinishShanheGuangao(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishShanheGuangao(frame);
    internal void PublishShanheGuangaoPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShanheGuangaoPrompt(frame);
    internal global::CardGame.Core.CardUseFrame RequireShanheGuangaoUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireShanheGuangaoUse(frame);
    internal void ResolveShanheGuangaoChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveShanheGuangaoChoice(selected);
    internal bool ResumeShanheGuangao(long frameId) => _resolve().ResumeShanheGuangao(frameId);
    internal void ReturnShanheGuangaoDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheGuangaoDraw(supplied, outcome);
    internal global::CardGame.Core.PromptChoice SelectAiShanheGuangao(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiShanheGuangao(decision, frame);
    internal int[] ShanheGuangaoCandidates(int owner, global::CardGame.Core.CardUseFrame use) => _resolve().ShanheGuangaoCandidates(owner, use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheGuangaoChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheGuangaoChoices(frame);
}
