#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitsEntries
{
    internal SkillProgramStepOutcome BeginFirstSlashDuelBenefit(ProgramSkillFrame supplied) => _resolve().BeginFirstSlashDuelBenefit(supplied);
    internal bool CanCollectFirstSlashDuelCandidate(int owner, SkillProgramTrigger trigger, CardActionContext action) => _resolve().CanCollectFirstSlashDuelCandidate(owner, trigger, action);
    internal bool CanContinueFirstSlashDuelBenefit(ProgramSkillFrame f) => _resolve().CanContinueFirstSlashDuelBenefit(f);
    internal bool CanOfferFirstSlashDuelBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferFirstSlashDuelBenefit(candidate, trigger, context);
    internal bool DrainFirstSlashDuelChildren(ProgramSkillFrame f) => _resolve().DrainFirstSlashDuelChildren(f);
    internal void FinishFirstSlashDuelBenefit(ProgramSkillFrame f) => _resolve().FinishFirstSlashDuelBenefit(f);
    internal IReadOnlyList<PromptChoice> FirstSlashDuelChoices(ProgramSkillFrame f) => _resolve().FirstSlashDuelChoices(f);
    internal int FirstSlashDuelDamage(GameEngine.IDamageAttempt attack) => _resolve().FirstSlashDuelDamage(attack);
    internal bool FirstSlashDuelDeclaredKindMatches(CardUseFrame use, CardKind declaredKind) => _resolve().FirstSlashDuelDeclaredKindMatches(use, declaredKind);
    internal CardConversionSource FirstSlashDuelSource(ProgramSkillFrame f) => _resolve().FirstSlashDuelSource(f);
    internal bool IsFirstSlashDuelAction(int owner, CardActionContext action, CardUseFrame use) => _resolve().IsFirstSlashDuelAction(owner, action, use);
    internal void ObserveFirstSlashDuelDeclaration(IGameEvent payload) => _resolve().ObserveFirstSlashDuelDeclaration(payload);
    internal void ResolveFirstSlashDuelChoice(PromptChoice selected) => _resolve().ResolveFirstSlashDuelChoice(selected);
    internal bool ResumeFirstSlashDuelBenefit(long id) => _resolve().ResumeFirstSlashDuelBenefit(id);
    internal bool ReturnFirstSlashDuelMovement(ProgramSkillFrame f) => _resolve().ReturnFirstSlashDuelMovement(f);
    internal PromptChoice SelectAiFirstSlashDuelBenefit(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiFirstSlashDuelBenefit(decision, f);
    internal bool HasFirstSlashDuelBenefitCapability { get => _resolve().HasFirstSlashDuelBenefitCapability; }
}
