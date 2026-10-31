#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanMobileEntries
{
    internal void AnswerMobileZhuHuanPingkou(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerMobileZhuHuanPingkou(chosen);
    internal void AssertMobileZhuHuanPingkou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileZhuHuanPingkou(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileZhuHuanPingkou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileZhuHuanPingkou(supplied);
    internal bool CanOfferMobileZhuHuanPingkou(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhuHuanPingkou(gate);
    internal void IssueMobileZhuHuanPingkouDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMobileZhuHuanPingkouDamage(f);
    internal bool KeepMobileZhuHuanPingkouIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileZhuHuanPingkouIssued(f);
    internal global::CardGame.Core.PromptChoice MobileZhuHuanPingkouAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileZhuHuanPingkouAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileZhuHuanPingkouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileZhuHuanPingkouChoices(f);
    internal void PublishMobileZhuHuanPingkou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileZhuHuanPingkou(f);
    internal bool ResumeMobileZhuHuanPingkou(long id) => _resolve().ResumeMobileZhuHuanPingkou(id);
    internal bool ValidMobileZhuHuanPingkou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileZhuHuanPingkou(f);
}
