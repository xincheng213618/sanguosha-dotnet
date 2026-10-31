#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCheZhouAnniversaryEntries
{
    internal bool AdvanceCheZhouSubtree() => _resolve().AdvanceCheZhouSubtree();
    internal int AnniversaryCheZhouAgeAt(global::CardGame.Core.CardUseFrame use, int round, long cutoff) => _resolve().AnniversaryCheZhouAgeAt(use, round, cutoff);
    internal void AnswerCheZhou(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerCheZhou(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryCheZhouPigua(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginAnniversaryCheZhouPigua(supplied);
    internal bool CanOfferAnniversaryCheZhou(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferAnniversaryCheZhou(candidate, trigger, c);
    internal void CaptureAnniversaryCheZhouAge(global::CardGame.Core.IGameEvent fact) => _resolve().CaptureAnniversaryCheZhouAge(fact);
    internal global::CardGame.Core.PromptChoice CheZhouAi(global::CardGame.Core.PendingDecision p) => _resolve().CheZhouAi(p);
    internal bool CheZhouBeforeDamageObserver(long id) => _resolve().CheZhouBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CheZhouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CheZhouChoices(f);
    internal bool CheZhouDamageObserver(long id) => _resolve().CheZhouDamageObserver(id);
    internal bool CheZhouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CheZhouFirstChild(f, child);
    internal bool CheZhouMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().CheZhouMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? CheZhouObserverRoot() => _resolve().CheZhouObserverRoot();
    internal bool ContinueCheZhou(long id) => _resolve().ContinueCheZhou(id);
    internal bool ContinuePaidCheZhou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCheZhou(f);
    internal bool ExactCheZhouDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactCheZhouDamage(f);
    internal void FinishCheZhou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCheZhou(f);
    internal bool IsAnniversaryCheZhouHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsAnniversaryCheZhouHandExempt(owner, card);
    internal void IssueCheZhouObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueCheZhouObtain(f);
    internal void PublishCheZhou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCheZhou(f);
    internal void RecordAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int previousAmount, int replacement) => _resolve().RecordAnniversaryCheZhouDamageReplacement(attack, previousAmount, replacement);
    internal bool ReturnCheZhouMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCheZhouMovement(f);
    internal bool TryGetAnniversaryCheZhouDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out int amount) => _resolve().TryGetAnniversaryCheZhouDamageReplacement(attack, out amount);
    internal bool ValidAnniversaryCheZhouUseAge(global::CardGame.Core.CardUseFrame use) => _resolve().ValidAnniversaryCheZhouUseAge(use);
    internal bool ValidCheZhou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCheZhou(f);
    internal bool HasAnniversaryCheZhouContent { get => _resolve().HasAnniversaryCheZhouContent; }
    internal bool HasAnniversaryCheZhouRules { get => _resolve().HasAnniversaryCheZhouRules; }
}
