#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShortRangeSlashTargetsEntries
{
    internal void AppendShortRangeLegacyCandidate(CardUseFrame use, long windowId, List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)> entries) => _resolve().AppendShortRangeLegacyCandidate(use, windowId, entries);
    internal bool CanRunShortRangeSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunShortRangeSlash(candidate, trigger, context);
    internal int FreezeShortRangeSlashResponseRequirement(long attackId, int source, int target, CardKind kind, int ordinary) => _resolve().FreezeShortRangeSlashResponseRequirement(attackId, source, target, kind, ordinary);
    internal bool HasShortRangeSlashTail(CardUseFrame use) => _resolve().HasShortRangeSlashTail(use);
    internal bool IsShortRangeLegacyCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsShortRangeLegacyCandidate(candidate, context);
    internal bool IsShortRangeSlashRedirectFact(CardUseFrame use, ProgramActualSlashTargetRedirectedEvent fact, IReadOnlyList<int> actualTargets) => _resolve().IsShortRangeSlashRedirectFact(use, fact, actualTargets);
    internal bool IsShortRangeSlashTargetFact(CardUseFrame use, ShortRangeSlashTargetResolvedEvent fact, IReadOnlyList<int> originalTargets) => _resolve().IsShortRangeSlashTargetFact(use, fact, originalTargets);
    internal SkillProgramStepOutcome OfferShortRangeSlashTarget(ProgramSkillFrame supplied) => _resolve().OfferShortRangeSlashTarget(supplied);
    internal void PublishShortRangeSlashTarget(ProgramSkillFrame frame) => _resolve().PublishShortRangeSlashTarget(frame);
    internal void RecordShortRangeSlashRedirect(ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordShortRangeSlashRedirect(redirector, useId, originalSeat, newSeat);
    internal void ResolveShortRangeSlashTarget(PromptChoice selected) => _resolve().ResolveShortRangeSlashTarget(selected);
    internal PromptChoice SelectAiShortRangeSlash(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiShortRangeSlash(decision, frame);
    internal IReadOnlyList<PromptChoice> ShortRangeSlashChoices(ProgramSkillFrame frame) => _resolve().ShortRangeSlashChoices(frame);
    internal CardConversionSource ShortRangeSlashSource(ProgramSkillFrame frame) => _resolve().ShortRangeSlashSource(frame);
    internal int[] ShortRangeSlashTargets(CardUseFrame use) => _resolve().ShortRangeSlashTargets(use);
    internal CardUseFrame? ShortRangeSlashUse(int owner, ProgramSkillWindowContext context) => _resolve().ShortRangeSlashUse(owner, context);
    internal bool ShortRangeSlashWasOffered(long useId) => _resolve().ShortRangeSlashWasOffered(useId);
    internal bool TryGetShortRangeFinalizedPrefix(CardUseFrame use, ShortRangeSlashTargetReceipt receipt, scoped out CardActionContext finalized) => _resolve().TryGetShortRangeFinalizedPrefix(use, receipt, out finalized);
    internal bool TryGetShortRangeVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> target) => _resolve().TryGetShortRangeVirtualPrimaryReturn(use, parent, out target);
    internal bool HasShortRangeSlashCapability { get => _resolve().HasShortRangeSlashCapability; }
}
