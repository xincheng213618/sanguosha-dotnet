#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSoleTargetProtection(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginSoleTargetProtection(input);
    internal bool CanContinueSoleTargetProtection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueSoleTargetProtection(f);
    internal bool CanOfferSoleTargetProtection(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSoleTargetProtection(candidate, trigger, context);
    internal bool CanSubstituteSoleProtection(global::CardGame.Core.ProgramSkillFrame f, bool afterDraw) => _resolve().CanSubstituteSoleProtection(f, afterDraw);
    internal bool HasExactSoleProtectionRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasExactSoleProtectionRoot(f);
    internal bool IsSoleProtectionReplacedActor(global::CardGame.Core.CardUseFrame use, int capturedActor) => _resolve().IsSoleProtectionReplacedActor(use, capturedActor);
    internal void PublishSoleProtectionChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSoleProtectionChoice(f);
    internal void QuarantineSoleProtectionUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QuarantineSoleProtectionUse(f);
    internal void ResolveSoleProtectionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSoleProtectionChoice(selected);
    internal bool ResumeSoleTargetProtection(long id) => _resolve().ResumeSoleTargetProtection(id);
    internal bool ReturnSoleTargetProtectionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSoleTargetProtectionMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiSoleProtection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSoleProtection(decision, f);
    internal global::CardGame.Core.PromptChoice SoleProtectionChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? cardId = default) => _resolve().SoleProtectionChoice(f, branch, label, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SoleProtectionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SoleProtectionChoices(f);
    internal global::CardGame.Core.Card[] SoleProtectionCosts(int owner, string skillId, string instanceId) => _resolve().SoleProtectionCosts(owner, skillId, instanceId);
    internal int[] SoleProtectionMainTargets(global::CardGame.Core.CardUseFrame use) => _resolve().SoleProtectionMainTargets(use);
    internal bool SoleTargetProtectionNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().SoleTargetProtectionNormalizeImplicitDrawTwoTargetWindow(query);
    internal void SubstituteSoleProtectionTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SubstituteSoleProtectionTarget(f);
    internal bool TryGetSoleProtectionUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetSoleProtectionUse(owner, context, out use, out window);
    internal bool HasSoleTargetProtectionCapability { get => _resolve().HasSoleTargetProtectionCapability; }
    internal bool HasSoleTargetProtectionParticipant { get => _resolve().HasSoleTargetProtectionParticipant; }
}
