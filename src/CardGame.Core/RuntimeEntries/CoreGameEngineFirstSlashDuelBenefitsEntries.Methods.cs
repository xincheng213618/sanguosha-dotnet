#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginFirstSlashDuelBenefit(supplied);
    internal bool CanCollectFirstSlashDuelCandidate(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action) => _resolve().CanCollectFirstSlashDuelCandidate(owner, trigger, action);
    internal bool CanContinueFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueFirstSlashDuelBenefit(f);
    internal bool CanOfferFirstSlashDuelBenefit(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferFirstSlashDuelBenefit(candidate, trigger, context);
    internal bool DrainFirstSlashDuelChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainFirstSlashDuelChildren(f);
    internal void FinishFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFirstSlashDuelBenefit(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FirstSlashDuelChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FirstSlashDuelChoices(f);
    internal int FirstSlashDuelDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FirstSlashDuelDamage(attack);
    internal bool FirstSlashDuelDeclaredKindMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardKind declaredKind) => _resolve().FirstSlashDuelDeclaredKindMatches(use, declaredKind);
    internal global::CardGame.Core.CardConversionSource FirstSlashDuelSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FirstSlashDuelSource(f);
    internal bool IsFirstSlashDuelAction(int owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardUseFrame use) => _resolve().IsFirstSlashDuelAction(owner, action, use);
    internal void ObserveFirstSlashDuelDeclaration(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveFirstSlashDuelDeclaration(payload);
    internal void ResolveFirstSlashDuelChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFirstSlashDuelChoice(selected);
    internal bool ResumeFirstSlashDuelBenefit(long id) => _resolve().ResumeFirstSlashDuelBenefit(id);
    internal bool ReturnFirstSlashDuelMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFirstSlashDuelMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiFirstSlashDuelBenefit(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiFirstSlashDuelBenefit(decision, f);
    internal bool HasFirstSlashDuelBenefitCapability { get => _resolve().HasFirstSlashDuelBenefitCapability; }
}
