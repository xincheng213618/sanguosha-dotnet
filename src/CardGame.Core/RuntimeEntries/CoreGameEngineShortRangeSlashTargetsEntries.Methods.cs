#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShortRangeSlashTargetsEntries
{
    internal void AppendShortRangeLegacyCandidate(global::CardGame.Core.CardUseFrame use, long windowId, global::System.Collections.Generic.List<(global::CardGame.Core.ProgramTriggerCandidate Candidate, global::CardGame.Core.ProgramSkillWindowContext Context)> entries) => _resolve().AppendShortRangeLegacyCandidate(use, windowId, entries);
    internal bool CanRunShortRangeSlash(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunShortRangeSlash(candidate, trigger, context);
    internal int FreezeShortRangeSlashResponseRequirement(long attackId, int source, int target, global::CardGame.Core.CardKind kind, int ordinary) => _resolve().FreezeShortRangeSlashResponseRequirement(attackId, source, target, kind, ordinary);
    internal bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasShortRangeSlashTail(use);
    internal bool IsShortRangeLegacyCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsShortRangeLegacyCandidate(candidate, context);
    internal bool IsShortRangeSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramActualSlashTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets) => _resolve().IsShortRangeSlashRedirectFact(use, fact, actualTargets);
    internal bool IsShortRangeSlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ShortRangeSlashTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().IsShortRangeSlashTargetFact(use, fact, originalTargets);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferShortRangeSlashTarget(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().OfferShortRangeSlashTarget(supplied);
    internal void PublishShortRangeSlashTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShortRangeSlashTarget(frame);
    internal void RecordShortRangeSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordShortRangeSlashRedirect(redirector, useId, originalSeat, newSeat);
    internal void ResolveShortRangeSlashTarget(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveShortRangeSlashTarget(selected);
    internal global::CardGame.Core.PromptChoice SelectAiShortRangeSlash(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiShortRangeSlash(decision, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShortRangeSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShortRangeSlashChoices(frame);
    internal global::CardGame.Core.CardConversionSource ShortRangeSlashSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShortRangeSlashSource(frame);
    internal int[] ShortRangeSlashTargets(global::CardGame.Core.CardUseFrame use) => _resolve().ShortRangeSlashTargets(use);
    internal global::CardGame.Core.CardUseFrame? ShortRangeSlashUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ShortRangeSlashUse(owner, context);
    internal bool ShortRangeSlashWasOffered(long useId) => _resolve().ShortRangeSlashWasOffered(useId);
    internal bool TryGetShortRangeFinalizedPrefix(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ShortRangeSlashTargetReceipt receipt, scoped out global::CardGame.Core.CardActionContext finalized) => _resolve().TryGetShortRangeFinalizedPrefix(use, receipt, out finalized);
    internal bool TryGetShortRangeVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> target) => _resolve().TryGetShortRangeVirtualPrimaryReturn(use, parent, out target);
    internal bool HasShortRangeSlashCapability { get => _resolve().HasShortRangeSlashCapability; }
}
