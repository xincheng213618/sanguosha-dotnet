#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMobileEntries
{
    internal void AnswerHpDifferencePileDamage(PromptChoice choice) => _resolve().AnswerHpDifferencePileDamage(choice);
    internal bool CanOfferMobileLuZhiCompletion(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferMobileLuZhiCompletion(candidate, context);
    internal void CaptureMobileLuZhiYin(ProgramConversionPolarityCommittedEvent fact) => _resolve().CaptureMobileLuZhiYin(fact);
    internal void ContinueHpDifferencePileDamage(long id) => _resolve().ContinueHpDifferencePileDamage(id);
    internal bool? HpDifferencePileCardColor(int owner, Card card) => _resolve().HpDifferencePileCardColor(owner, card);
    internal PromptChoice HpDifferencePileChoice(ProgramSkillFrame f, string branch, int value, string label) => _resolve().HpDifferencePileChoice(f, branch, value, label);
    internal IReadOnlyList<PromptChoice> HpDifferencePileChoices(ProgramSkillFrame f) => _resolve().HpDifferencePileChoices(f);
    internal Card[] HpDifferencePileCosts(int owner, string skill, string instance, bool red) => _resolve().HpDifferencePileCosts(owner, skill, instance, red);
    internal IReadOnlyList<int> HpDifferencePileDamageTargets(int owner, string skill, string instance, SkillProgramEffect effect) => _resolve().HpDifferencePileDamageTargets(owner, skill, instance, effect);
    internal int HpDifferencePileX(int owner, int target) => _resolve().HpDifferencePileX(owner, target);
    internal bool IsMobileLuZhiTrueResponseUse(CardActionContext action) => _resolve().IsMobileLuZhiTrueResponseUse(action);
    internal bool KeepMobileLuZhiPaid(ProgramSkillFrame f) => _resolve().KeepMobileLuZhiPaid(f);
    internal bool MobileLuZhiCompletion(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool committed = false) => _resolve().MobileLuZhiCompletion(candidate, context, committed);
    internal void PublishHpDifferencePileDamage(ProgramSkillFrame f) => _resolve().PublishHpDifferencePileDamage(f);
    internal bool ResumeHpDifferencePileDamage(long id) => _resolve().ResumeHpDifferencePileDamage(id);
    internal bool ReturnHpDifferencePileDamage(ProgramSkillFrame f) => _resolve().ReturnHpDifferencePileDamage(f);
    internal SkillProgramStepOutcome StartHpDifferencePileDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().StartHpDifferencePileDamage(frame, effect);
}
