#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLihuo3160Entries
{
    internal PromptChoice AiShanheSourceLihuoCost(PendingDecision p) => _resolve().AiShanheSourceLihuoCost(p);
    internal void AnswerShanheSourceLihuoCost(PromptChoice choice) => _resolve().AnswerShanheSourceLihuoCost(choice);
    internal SkillProgramStepOutcome BeginShanheSourceLihuoCost(ProgramSkillFrame supplied) => _resolve().BeginShanheSourceLihuoCost(supplied);
    internal bool CanOfferShanheSourceLihuoCost(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferShanheSourceLihuoCost(c, t, w);
    internal void PublishShanheSourceLihuoCost(ProgramSkillFrame f) => _resolve().PublishShanheSourceLihuoCost(f);
    internal bool ResumeShanheSourceLihuoCost(long id) => _resolve().ResumeShanheSourceLihuoCost(id);
    internal IReadOnlyList<PromptChoice> ShanheSourceLihuoCostChoices(ProgramSkillFrame f) => _resolve().ShanheSourceLihuoCostChoices(f);
    internal CardUseFrame? ShanheSourceLihuoDamageUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().ShanheSourceLihuoDamageUse(c, w);
    internal int ShanheSourceLihuoProspectiveTargetBonus(CardUseFrame use) => _resolve().ShanheSourceLihuoProspectiveTargetBonus(use);
    internal void UnlockShanheSourceLihuoTarget(IGameEvent fact) => _resolve().UnlockShanheSourceLihuoTarget(fact);
}
