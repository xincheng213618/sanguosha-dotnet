#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionEntries
{
    internal SkillProgramStepOutcome BeginSoleTargetProtection(ProgramSkillFrame input) => _resolve().BeginSoleTargetProtection(input);
    internal bool CanContinueSoleTargetProtection(ProgramSkillFrame f) => _resolve().CanContinueSoleTargetProtection(f);
    internal bool CanOfferSoleTargetProtection(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSoleTargetProtection(candidate, trigger, context);
    internal bool CanSubstituteSoleProtection(ProgramSkillFrame f, bool afterDraw) => _resolve().CanSubstituteSoleProtection(f, afterDraw);
    internal bool HasExactSoleProtectionRoot(ProgramSkillFrame f) => _resolve().HasExactSoleProtectionRoot(f);
    internal bool IsSoleProtectionReplacedActor(CardUseFrame use, int capturedActor) => _resolve().IsSoleProtectionReplacedActor(use, capturedActor);
    internal void PublishSoleProtectionChoice(ProgramSkillFrame f) => _resolve().PublishSoleProtectionChoice(f);
    internal void QuarantineSoleProtectionUse(ProgramSkillFrame f) => _resolve().QuarantineSoleProtectionUse(f);
    internal void ResolveSoleProtectionChoice(PromptChoice selected) => _resolve().ResolveSoleProtectionChoice(selected);
    internal bool ResumeSoleTargetProtection(long id) => _resolve().ResumeSoleTargetProtection(id);
    internal bool ReturnSoleTargetProtectionMovement(ProgramSkillFrame f) => _resolve().ReturnSoleTargetProtectionMovement(f);
    internal PromptChoice SelectAiSoleProtection(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSoleProtection(decision, f);
    internal PromptChoice SoleProtectionChoice(ProgramSkillFrame f, string branch, string label, int? cardId = default) => _resolve().SoleProtectionChoice(f, branch, label, cardId);
    internal IReadOnlyList<PromptChoice> SoleProtectionChoices(ProgramSkillFrame f) => _resolve().SoleProtectionChoices(f);
    internal Card[] SoleProtectionCosts(int owner, string skillId, string instanceId) => _resolve().SoleProtectionCosts(owner, skillId, instanceId);
    internal int[] SoleProtectionMainTargets(CardUseFrame use) => _resolve().SoleProtectionMainTargets(use);
    internal bool SoleTargetProtectionNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().SoleTargetProtectionNormalizeImplicitDrawTwoTargetWindow(query);
    internal void SubstituteSoleProtectionTarget(ProgramSkillFrame f) => _resolve().SubstituteSoleProtectionTarget(f);
    internal bool TryGetSoleProtectionUse(int owner, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetSoleProtectionUse(owner, context, out use, out window);
    internal bool HasSoleTargetProtectionCapability { get => _resolve().HasSoleTargetProtectionCapability; }
    internal bool HasSoleTargetProtectionParticipant { get => _resolve().HasSoleTargetProtectionParticipant; }
}
