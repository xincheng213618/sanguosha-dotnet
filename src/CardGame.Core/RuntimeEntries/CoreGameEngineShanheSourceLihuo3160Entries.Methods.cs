#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLihuo3160Entries
{
    internal global::CardGame.Core.PromptChoice AiShanheSourceLihuoCost(global::CardGame.Core.PendingDecision p) => _resolve().AiShanheSourceLihuoCost(p);
    internal void AnswerShanheSourceLihuoCost(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShanheSourceLihuoCost(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheSourceLihuoCost(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheSourceLihuoCost(supplied);
    internal bool CanOfferShanheSourceLihuoCost(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferShanheSourceLihuoCost(c, t, w);
    internal void PublishShanheSourceLihuoCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShanheSourceLihuoCost(f);
    internal bool ResumeShanheSourceLihuoCost(long id) => _resolve().ResumeShanheSourceLihuoCost(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheSourceLihuoCostChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShanheSourceLihuoCostChoices(f);
    internal global::CardGame.Core.CardUseFrame? ShanheSourceLihuoDamageUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShanheSourceLihuoDamageUse(c, w);
    internal int ShanheSourceLihuoProspectiveTargetBonus(global::CardGame.Core.CardUseFrame use) => _resolve().ShanheSourceLihuoProspectiveTargetBonus(use);
    internal void UnlockShanheSourceLihuoTarget(global::CardGame.Core.IGameEvent fact) => _resolve().UnlockShanheSourceLihuoTarget(fact);
}
