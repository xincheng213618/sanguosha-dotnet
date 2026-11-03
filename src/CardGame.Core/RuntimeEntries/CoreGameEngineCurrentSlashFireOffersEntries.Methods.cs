#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentSlashFireOffersEntries
{
    internal void AssertCurrentSlashFireDraft(ProgramSkillFrame f) => _resolve().AssertCurrentSlashFireDraft(f);
    internal void AssertCurrentSlashFirePolicy(CardUseFrame use) => _resolve().AssertCurrentSlashFirePolicy(use);
    internal SkillProgramStepOutcome BeginCurrentSlashFireOffer(ProgramSkillFrame input) => _resolve().BeginCurrentSlashFireOffer(input);
    internal bool CanAddCurrentFireSlashTarget(CardUseFrame use) => _resolve().CanAddCurrentFireSlashTarget(use);
    internal bool CanOfferCurrentSlashFire(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferCurrentSlashFire(candidate, trigger, context);
    internal int[] CurrentFireSlashExtraTargets(CardUseFrame use) => _resolve().CurrentFireSlashExtraTargets(use);
    internal int CurrentFireSlashMaximum(CardUseFrame use) => _resolve().CurrentFireSlashMaximum(use);
    internal bool CurrentFireSlashOriginalTargetsAreLegal(CardUseFrame use) => _resolve().CurrentFireSlashOriginalTargetsAreLegal(use);
    internal IReadOnlyList<PromptChoice> CurrentSlashFireChoices(ProgramSkillFrame f) => _resolve().CurrentSlashFireChoices(f);
    internal CardUseFrame CurrentSlashFireUse(ProgramSkillFrame f) => _resolve().CurrentSlashFireUse(f);
    internal bool HasCommittedSlashFireCapability(CharacterState owner) => _resolve().HasCommittedSlashFireCapability(owner);
    internal bool IsCurrentSlashFireChangedUse(CardUseFrame use) => _resolve().IsCurrentSlashFireChangedUse(use);
    internal void PublishCurrentSlashFirePrompt(ProgramSkillFrame f) => _resolve().PublishCurrentSlashFirePrompt(f);
    internal void ResolveCurrentSlashFireChoice(PromptChoice selected) => _resolve().ResolveCurrentSlashFireChoice(selected);
}
