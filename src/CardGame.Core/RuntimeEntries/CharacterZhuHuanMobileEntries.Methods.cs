#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanMobileEntries
{
    internal void AnswerMobileZhuHuanPingkou(PromptChoice chosen) => _resolve().AnswerMobileZhuHuanPingkou(chosen);
    internal SkillProgramStepOutcome BeginMobileZhuHuanPingkou(ProgramSkillFrame supplied) => _resolve().BeginMobileZhuHuanPingkou(supplied);
    internal bool CanOfferMobileZhuHuanPingkou(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhuHuanPingkou(gate);
    internal void IssueMobileZhuHuanPingkouDamage(ProgramSkillFrame f) => _resolve().IssueMobileZhuHuanPingkouDamage(f);
    internal bool KeepMobileZhuHuanPingkouIssued(ProgramSkillFrame f) => _resolve().KeepMobileZhuHuanPingkouIssued(f);
    internal PromptChoice MobileZhuHuanPingkouAi(PendingDecision p, ProgramSkillFrame f) => _resolve().MobileZhuHuanPingkouAi(p, f);
    internal IReadOnlyList<PromptChoice> MobileZhuHuanPingkouChoices(ProgramSkillFrame f) => _resolve().MobileZhuHuanPingkouChoices(f);
    internal void PublishMobileZhuHuanPingkou(ProgramSkillFrame f) => _resolve().PublishMobileZhuHuanPingkou(f);
    internal bool ResumeMobileZhuHuanPingkou(long id) => _resolve().ResumeMobileZhuHuanPingkou(id);
    internal bool ValidMobileZhuHuanPingkou(ProgramSkillFrame f) => _resolve().ValidMobileZhuHuanPingkou(f);
}
