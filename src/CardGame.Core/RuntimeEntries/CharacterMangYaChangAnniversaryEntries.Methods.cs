#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMangYaChangAnniversaryEntries
{
    internal bool AdmitAnniversaryJiedaoCost(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().AdmitAnniversaryJiedaoCost(c, w);
    internal PromptChoice AiAnniversaryJiedao(PendingDecision prompt, ProgramSkillFrame f) => _resolve().AiAnniversaryJiedao(prompt, f);
    internal void AnswerAnniversaryJiedao(PromptChoice choice) => _resolve().AnswerAnniversaryJiedao(choice);
    internal void AppendAnniversaryJiedaoDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow timing, List<DamageTriggerCandidate> candidates) => _resolve().AppendAnniversaryJiedaoDamageCandidates(attack, timing, candidates);
    internal SkillProgramStepOutcome BeginAnniversaryJiedao(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversaryJiedao(supplied, operation);
    internal bool CanOfferAnniversaryJiedao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryJiedao(c, t, w);
    internal void ContinueAnniversaryJiedao(long id) => _resolve().ContinueAnniversaryJiedao(id);
    internal bool ContinueIssuedAnniversaryJiedao(ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryJiedao(f);
    internal void FinishAnniversaryJiedao(ProgramSkillFrame f) => _resolve().FinishAnniversaryJiedao(f);
    internal bool FirstActualJiedaoSourceDamage(int seat) => _resolve().FirstActualJiedaoSourceDamage(seat);
    internal int JiedaoChainCursor(GameEngine.IDamageAttempt attack) => _resolve().JiedaoChainCursor(attack);
    internal IReadOnlyList<PromptChoice> JiedaoChoices(ProgramSkillFrame f) => _resolve().JiedaoChoices(f);
    internal AnniversaryJiedaoFrame? JiedaoCostWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().JiedaoCostWitness(c, w);
    internal IReadOnlyList<Card> JiedaoDiscardableCards(ProgramSkillFrame f) => _resolve().JiedaoDiscardableCards(f);
    internal bool JiedaoFinalRecipientMatches(AnniversaryJiedaoGrant grant, GameEngine.IDamageAttempt attack) => _resolve().JiedaoFinalRecipientMatches(grant, attack);
    internal bool JiedaoPaymentMatches(ProgramSkillFrame f) => _resolve().JiedaoPaymentMatches(f);
    internal int JiedaoSourceDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().JiedaoSourceDamageBonus(attack);
    internal AnniversaryJiedaoSourceReceipt? JiedaoSourceWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().JiedaoSourceWitness(c, w);
    internal void PublishJiedaoChoice(ProgramSkillFrame f) => _resolve().PublishJiedaoChoice(f);
    internal bool ResumeAnniversaryJiedao(long id) => _resolve().ResumeAnniversaryJiedao(id);
    internal bool ResumeAnniversaryJiedaoSourceWindow(BeforeDamageProgramWindowFrame window, GameEngine.IDamageAttempt attack) => _resolve().ResumeAnniversaryJiedaoSourceWindow(window, attack);
    internal bool ReturnAnniversaryJiedaoMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryJiedaoMovement(f);
    internal bool TryBeginAnniversaryJiedaoSourceWindow(GameEngine.IDamageAttempt attack) => _resolve().TryBeginAnniversaryJiedaoSourceWindow(attack);
    internal bool ValidJiedaoFrame(ProgramSkillFrame f) => _resolve().ValidJiedaoFrame(f);
    internal bool ValidJiedaoGrant(AnniversaryJiedaoGrant g) => _resolve().ValidJiedaoGrant(g);
    internal bool ValidJiedaoOrigin(AnniversaryJiedaoSourceReceipt r) => _resolve().ValidJiedaoOrigin(r);
    internal bool HasAnniversaryJiedao { get => _resolve().HasAnniversaryJiedao; }
}
