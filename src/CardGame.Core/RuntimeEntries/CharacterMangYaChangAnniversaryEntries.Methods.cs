#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMangYaChangAnniversaryEntries
{
    internal bool AdmitAnniversaryJiedaoCost(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AdmitAnniversaryJiedaoCost(c, w);
    internal global::CardGame.Core.PromptChoice AiAnniversaryJiedao(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnniversaryJiedao(prompt, f);
    internal void AnswerAnniversaryJiedao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryJiedao(choice);
    internal void AppendAnniversaryJiedaoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow timing, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => _resolve().AppendAnniversaryJiedaoDamageCandidates(attack, timing, candidates);
    internal void AssertJiedaoFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertJiedaoFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryJiedao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversaryJiedao(supplied, operation);
    internal bool CanOfferAnniversaryJiedao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryJiedao(c, t, w);
    internal void ContinueAnniversaryJiedao(long id) => _resolve().ContinueAnniversaryJiedao(id);
    internal bool ContinueIssuedAnniversaryJiedao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryJiedao(f);
    internal void FinishAnniversaryJiedao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnniversaryJiedao(f);
    internal bool FirstActualJiedaoSourceDamage(int seat) => _resolve().FirstActualJiedaoSourceDamage(seat);
    internal bool HasAnniversaryJiedaoPrompt() => _resolve().HasAnniversaryJiedaoPrompt();
    internal int JiedaoChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().JiedaoChainCursor(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> JiedaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JiedaoChoices(f);
    internal global::CardGame.Core.AnniversaryJiedaoFrame? JiedaoCostWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().JiedaoCostWitness(c, w);
    internal bool JiedaoDiscardMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().JiedaoDiscardMovementProof(f, effect, pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> JiedaoDiscardableCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JiedaoDiscardableCards(f);
    internal bool JiedaoFinalRecipientMatches(global::CardGame.Core.AnniversaryJiedaoGrant grant, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().JiedaoFinalRecipientMatches(grant, attack);
    internal bool JiedaoPaymentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JiedaoPaymentMatches(f);
    internal int JiedaoSourceDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().JiedaoSourceDamageBonus(attack);
    internal global::CardGame.Core.AnniversaryJiedaoSourceReceipt? JiedaoSourceWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().JiedaoSourceWitness(c, w);
    internal void PublishJiedaoChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishJiedaoChoice(f);
    internal bool ResumeAnniversaryJiedao(long id) => _resolve().ResumeAnniversaryJiedao(id);
    internal bool ResumeAnniversaryJiedaoSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame window, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ResumeAnniversaryJiedaoSourceWindow(window, attack);
    internal bool ReturnAnniversaryJiedaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryJiedaoMovement(f);
    internal bool TryBeginAnniversaryJiedaoSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().TryBeginAnniversaryJiedaoSourceWindow(attack);
    internal bool ValidJiedaoFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidJiedaoFrame(f);
    internal bool ValidJiedaoGrant(global::CardGame.Core.AnniversaryJiedaoGrant g) => _resolve().ValidJiedaoGrant(g);
    internal bool ValidJiedaoOrigin(global::CardGame.Core.AnniversaryJiedaoSourceReceipt r) => _resolve().ValidJiedaoOrigin(r);
    internal bool HasAnniversaryJiedao { get => _resolve().HasAnniversaryJiedao; }
}
