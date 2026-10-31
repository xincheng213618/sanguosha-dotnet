#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMobileEntries
{
    internal void AnswerHpDifferencePileDamage(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHpDifferencePileDamage(choice);
    internal void AssertMobileLuZhiFrames(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileLuZhiFrames(f);
    internal bool CanOfferMobileLuZhiCompletion(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMobileLuZhiCompletion(candidate, context);
    internal void CaptureMobileLuZhiYin(global::CardGame.Core.ProgramConversionPolarityCommittedEvent fact) => _resolve().CaptureMobileLuZhiYin(fact);
    internal void ContinueHpDifferencePileDamage(long id) => _resolve().ContinueHpDifferencePileDamage(id);
    internal bool? HpDifferencePileCardColor(int owner, global::CardGame.Core.Card card) => _resolve().HpDifferencePileCardColor(owner, card);
    internal global::CardGame.Core.PromptChoice HpDifferencePileChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, int value, string label) => _resolve().HpDifferencePileChoice(f, branch, value, label);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HpDifferencePileChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HpDifferencePileChoices(f);
    internal global::CardGame.Core.Card[] HpDifferencePileCosts(int owner, string skill, string instance, bool red) => _resolve().HpDifferencePileCosts(owner, skill, instance, red);
    internal global::System.Collections.Generic.IReadOnlyList<int> HpDifferencePileDamageTargets(int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect effect) => _resolve().HpDifferencePileDamageTargets(owner, skill, instance, effect);
    internal int HpDifferencePileX(int owner, int target) => _resolve().HpDifferencePileX(owner, target);
    internal bool IsMobileLuZhiTrueResponseUse(global::CardGame.Core.CardActionContext action) => _resolve().IsMobileLuZhiTrueResponseUse(action);
    internal bool KeepMobileLuZhiPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileLuZhiPaid(f);
    internal bool MobileLuZhiCompletion(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, bool committed = false) => _resolve().MobileLuZhiCompletion(candidate, context, committed);
    internal void PublishHpDifferencePileDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHpDifferencePileDamage(f);
    internal bool ResumeHpDifferencePileDamage(long id) => _resolve().ResumeHpDifferencePileDamage(id);
    internal bool ReturnHpDifferencePileDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHpDifferencePileDamage(f);
    internal global::CardGame.Core.SkillProgramStepOutcome StartHpDifferencePileDamage(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().StartHpDifferencePileDamage(frame, effect);
}
