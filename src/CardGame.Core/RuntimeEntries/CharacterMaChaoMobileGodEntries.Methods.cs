#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoMobileGodEntries
{
    internal void AnswerMobileMaChao(PromptChoice c) => _resolve().AnswerMobileMaChao(c);
    internal SkillProgramStepOutcome BeginMobileMaChao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileMaChao(supplied, op);
    internal bool CanMobileJimie(int owner) => _resolve().CanMobileJimie(owner);
    internal PromptChoice ChooseMobileJimie(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMobileJimie(p, f);
    internal PromptChoice ChooseMobileTingwei(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMobileTingwei(p, f);
    internal PromptChoice ChooseMobileYuli(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMobileYuli(p, f);
    internal void ContinueMobileMaChao(long id) => _resolve().ContinueMobileMaChao(id);
    internal bool DrainMobileMaChao(long id) => _resolve().DrainMobileMaChao(id);
    internal void ExpireMobileTingwei(IGameEvent fact) => _resolve().ExpireMobileTingwei(fact);
    internal void FinishMobileMaChao(ProgramSkillFrame f) => _resolve().FinishMobileMaChao(f);
    internal bool IsMobileMaChaoTingweiGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsMobileMaChaoTingweiGrantQualified(owner, grant);
    internal long LastMobileJimieUse(int owner) => _resolve().LastMobileJimieUse(owner);
    internal CardActionContext? MobileMaChaoAction(ProgramSkillWindowContext? c) => _resolve().MobileMaChaoAction(c);
    internal PromptChoice MobileMaChaoChoice(ProgramSkillFrame f, string option, string text, int? target = default) => _resolve().MobileMaChaoChoice(f, option, text, target);
    internal IReadOnlyList<PromptChoice> MobileMaChaoChoices(ProgramSkillFrame f) => _resolve().MobileMaChaoChoices(f);
    internal Card[] MobileMaChaoDiscardable(int source, int target) => _resolve().MobileMaChaoDiscardable(source, target);
    internal int MobileMaChaoSourceDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MobileMaChaoSourceDamageBonus(attack);
    internal bool MobileMaChaoTriggerEligible(ProgramTriggerLegalityContext g) => _resolve().MobileMaChaoTriggerEligible(g);
    internal DamageNature MobileMaChaoYuliNature(GameEngine.IDamageAttempt attack, DamageNature priorNature) => _resolve().MobileMaChaoYuliNature(attack, priorNature);
    internal int MobileTingweiCardDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MobileTingweiCardDamageBonus(attack);
    internal void PublishMobileMaChao(ProgramSkillFrame f) => _resolve().PublishMobileMaChao(f);
    internal void RecordMobileYuli(ProgramSkillFrame f, int part) => _resolve().RecordMobileYuli(f, part);
    internal bool ResumeMobileMaChao(long id) => _resolve().ResumeMobileMaChao(id);
    internal bool RetainMobileMaChao(ProgramSkillFrame f) => _resolve().RetainMobileMaChao(f);
    internal void ReturnMobileMaChaoDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMobileMaChaoDraw(f, o);
    internal bool ReturnMobileMaChaoMovement(ProgramSkillFrame f) => _resolve().ReturnMobileMaChaoMovement(f);
    internal bool HasMobileShenMaChao { get => _resolve().HasMobileShenMaChao; }
}
