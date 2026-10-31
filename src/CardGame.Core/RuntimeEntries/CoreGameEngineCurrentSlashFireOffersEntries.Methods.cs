#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentSlashFireOffersEntries
{
    internal void AddCurrentNonFireConvertedTargetActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId = default) => _resolve().AddCurrentNonFireConvertedTargetActions(actions, actor, selectedPhysicalCardId);
    internal void AssertCurrentSlashFireDraft(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertCurrentSlashFireDraft(f);
    internal void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use) => _resolve().AssertCurrentSlashFirePolicy(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCurrentSlashFireOffer(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginCurrentSlashFireOffer(input);
    internal bool CanAddCurrentFireSlashTarget(global::CardGame.Core.CardUseFrame use) => _resolve().CanAddCurrentFireSlashTarget(use);
    internal bool CanOfferCurrentSlashFire(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCurrentSlashFire(candidate, trigger, context);
    internal int[] CurrentFireSlashExtraTargets(global::CardGame.Core.CardUseFrame use) => _resolve().CurrentFireSlashExtraTargets(use);
    internal int CurrentFireSlashMaximum(global::CardGame.Core.CardUseFrame use) => _resolve().CurrentFireSlashMaximum(use);
    internal bool CurrentFireSlashOriginalTargetsAreLegal(global::CardGame.Core.CardUseFrame use) => _resolve().CurrentFireSlashOriginalTargetsAreLegal(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CurrentSlashFireChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CurrentSlashFireChoices(f);
    internal global::CardGame.Core.CardUseFrame CurrentSlashFireUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CurrentSlashFireUse(f);
    internal bool HasCommittedSlashFireCapability(global::CardGame.Core.CharacterState owner) => _resolve().HasCommittedSlashFireCapability(owner);
    internal bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsCurrentSlashFireChangedUse(use);
    internal void PublishCurrentSlashFirePrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCurrentSlashFirePrompt(f);
    internal (int[] Designated, int[] Actual, int[] Planned) RebuildCurrentSlashFireTargets(global::CardGame.Core.CardUseFrame use) => _resolve().RebuildCurrentSlashFireTargets(use);
    internal void RecordCurrentSlashFireRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordCurrentSlashFireRedirect(redirector, useId, originalSeat, newSeat);
    internal void ResolveCurrentSlashFireChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveCurrentSlashFireChoice(selected);
    internal bool TryGetCurrentSlashFirePrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primary) => _resolve().TryGetCurrentSlashFirePrimaryReturn(use, parent, out primary);
}
