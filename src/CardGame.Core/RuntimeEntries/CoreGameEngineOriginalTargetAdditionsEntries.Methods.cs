#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalTargetAdditionsEntries
{
    internal void AssertOriginalTargetAdditionDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertOriginalTargetAdditionDraft(frame, paused);
    internal bool CanAddIssuedOriginalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OriginalTargetAdditionGrant grant) => _resolve().CanAddIssuedOriginalTarget(use, grant);
    internal bool CanContinueIssuedOriginalTargetAddition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedOriginalTargetAddition(frame);
    internal bool CanOfferOriginalTargetAddition(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOriginalTargetAddition(candidate, trigger, context);
    internal void CollectIssuedOriginalTargetCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedOriginalTargetCandidates(action, window, result);
    internal int CompletedFangtianOriginalTargetIndex(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi) => _resolve().CompletedFangtianOriginalTargetIndex(multi);
    internal void ExpireOriginalTargetAdditionGrants(int turn, int seat) => _resolve().ExpireOriginalTargetAdditionGrants(turn, seat);
    internal void GrantOriginalTargetAddition(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().GrantOriginalTargetAddition(frame, targetSeat);
    internal bool HasIssuedOriginalTargetAdditionTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasIssuedOriginalTargetAdditionTail(use);
    internal bool HasIssuedOriginalTargetArmorBypass(long id, int targetSeat) => _resolve().HasIssuedOriginalTargetArmorBypass(id, targetSeat);
    internal bool HasIssuedOriginalTargetCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedOriginalTargetCandidate(candidate, context);
    internal void IssueOriginalTargetAdditionPolicy(long id, global::CardGame.Core.CardActionContext? action) => _resolve().IssueOriginalTargetAdditionPolicy(id, action);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferOriginalTargetAddition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OfferOriginalTargetAddition(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OriginalTargetAdditionChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OriginalTargetAdditionChoices(frame);
    internal bool OriginalTargetAdditionFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().OriginalTargetAdditionFinalizedInstantTrickRequirementTargetWindow(query);
    internal int[] OriginalTargetAdditionTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OriginalTargetAdditionTargets(frame);
    internal void ResolveOriginalTargetAdditionChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveOriginalTargetAdditionChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiOriginalTargetAddition(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiOriginalTargetAddition(decision, frame);
    internal bool TryGetOriginalTargetVirtualSlashReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget) => _resolve().TryGetOriginalTargetVirtualSlashReturn(use, parent, out primaryTarget);
}
