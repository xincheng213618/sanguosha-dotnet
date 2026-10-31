#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCheZhouAnniversaryEntries
{
    internal bool AdvanceCheZhouSubtree() => _resolve().AdvanceCheZhouSubtree();
    internal int AnniversaryCheZhouAgeAt(CardUseFrame use, int round, long cutoff) => _resolve().AnniversaryCheZhouAgeAt(use, round, cutoff);
    internal void AnswerCheZhou(PromptChoice answer) => _resolve().AnswerCheZhou(answer);
    internal SkillProgramStepOutcome BeginAnniversaryCheZhouPigua(ProgramSkillFrame supplied) => _resolve().BeginAnniversaryCheZhouPigua(supplied);
    internal bool CanOfferAnniversaryCheZhou(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferAnniversaryCheZhou(candidate, trigger, c);
    internal void CaptureAnniversaryCheZhouAge(IGameEvent fact) => _resolve().CaptureAnniversaryCheZhouAge(fact);
    internal PromptChoice CheZhouAi(PendingDecision p) => _resolve().CheZhouAi(p);
    internal IReadOnlyList<PromptChoice> CheZhouChoices(ProgramSkillFrame f) => _resolve().CheZhouChoices(f);
    internal bool CheZhouFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CheZhouFirstChild(f, child);
    internal ProgramSkillFrame? CheZhouObserverRoot() => _resolve().CheZhouObserverRoot();
    internal bool ContinueCheZhou(long id) => _resolve().ContinueCheZhou(id);
    internal bool ContinuePaidCheZhou(ProgramSkillFrame f) => _resolve().ContinuePaidCheZhou(f);
    internal bool ExactCheZhouDamage(ProgramSkillFrame f) => _resolve().ExactCheZhouDamage(f);
    internal void FinishCheZhou(ProgramSkillFrame f) => _resolve().FinishCheZhou(f);
    internal bool IsAnniversaryCheZhouHandExempt(CharacterState owner, Card card) => _resolve().IsAnniversaryCheZhouHandExempt(owner, card);
    internal void IssueCheZhouObtain(ProgramSkillFrame f) => _resolve().IssueCheZhouObtain(f);
    internal void PublishCheZhou(ProgramSkillFrame f) => _resolve().PublishCheZhou(f);
    internal void RecordAnniversaryCheZhouDamageReplacement(GameEngine.IDamageAttempt attack, int previousAmount, int replacement) => _resolve().RecordAnniversaryCheZhouDamageReplacement(attack, previousAmount, replacement);
    internal bool ReturnCheZhouMovement(ProgramSkillFrame f) => _resolve().ReturnCheZhouMovement(f);
    internal bool TryGetAnniversaryCheZhouDamageReplacement(GameEngine.IDamageAttempt attack, scoped out int amount) => _resolve().TryGetAnniversaryCheZhouDamageReplacement(attack, out amount);
    internal bool ValidAnniversaryCheZhouUseAge(CardUseFrame use) => _resolve().ValidAnniversaryCheZhouUseAge(use);
    internal bool ValidCheZhou(ProgramSkillFrame f) => _resolve().ValidCheZhou(f);
    internal bool HasAnniversaryCheZhouContent { get => _resolve().HasAnniversaryCheZhouContent; }
    internal bool HasAnniversaryCheZhouRules { get => _resolve().HasAnniversaryCheZhouRules; }
}
