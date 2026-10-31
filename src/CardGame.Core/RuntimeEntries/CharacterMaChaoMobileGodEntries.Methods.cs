#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoMobileGodEntries
{
    internal void AnswerMobileMaChao(global::CardGame.Core.PromptChoice c) => _resolve().AnswerMobileMaChao(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileMaChao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileMaChao(supplied, op);
    internal bool CanMobileJimie(int owner) => _resolve().CanMobileJimie(owner);
    internal global::CardGame.Core.PromptChoice ChooseMobileJimie(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileJimie(p, f);
    internal global::CardGame.Core.PromptChoice ChooseMobileTingwei(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileTingwei(p, f);
    internal global::CardGame.Core.PromptChoice ChooseMobileYuli(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileYuli(p, f);
    internal void ContinueMobileMaChao(long id) => _resolve().ContinueMobileMaChao(id);
    internal bool DrainMobileMaChao(long id) => _resolve().DrainMobileMaChao(id);
    internal void ExpireMobileTingwei(global::CardGame.Core.IGameEvent fact) => _resolve().ExpireMobileTingwei(fact);
    internal void FinishMobileMaChao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileMaChao(f);
    internal bool IsMobileMaChaoTingweiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsMobileMaChaoTingweiGrantQualified(owner, grant);
    internal long LastMobileJimieUse(int owner) => _resolve().LastMobileJimieUse(owner);
    internal global::CardGame.Core.CardActionContext? MobileMaChaoAction(global::CardGame.Core.ProgramSkillWindowContext? c) => _resolve().MobileMaChaoAction(c);
    internal global::CardGame.Core.PromptChoice MobileMaChaoChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string text, int? target = default) => _resolve().MobileMaChaoChoice(f, option, text, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileMaChaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileMaChaoChoices(f);
    internal global::CardGame.Core.Card[] MobileMaChaoDiscardable(int source, int target) => _resolve().MobileMaChaoDiscardable(source, target);
    internal int MobileMaChaoSourceDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileMaChaoSourceDamageBonus(attack);
    internal bool MobileMaChaoTriggerEligible(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().MobileMaChaoTriggerEligible(g);
    internal global::CardGame.Core.DamageNature MobileMaChaoYuliNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature priorNature) => _resolve().MobileMaChaoYuliNature(attack, priorNature);
    internal int MobileTingweiCardDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileTingweiCardDamageBonus(attack);
    internal void PublishMobileMaChao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileMaChao(f);
    internal void RecordMobileYuli(global::CardGame.Core.ProgramSkillFrame f, int part) => _resolve().RecordMobileYuli(f, part);
    internal bool ResumeMobileMaChao(long id) => _resolve().ResumeMobileMaChao(id);
    internal bool RetainMobileMaChao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainMobileMaChao(f);
    internal void ReturnMobileMaChaoDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMobileMaChaoDraw(f, o);
    internal bool ReturnMobileMaChaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileMaChaoMovement(f);
    internal bool HasMobileShenMaChao { get => _resolve().HasMobileShenMaChao; }
}
