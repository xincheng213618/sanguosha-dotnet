#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalTargetAdditionsEntries
{
    internal bool CanAddIssuedOriginalTarget(CardUseFrame use, OriginalTargetAdditionGrant grant) => _resolve().CanAddIssuedOriginalTarget(use, grant);
    internal bool CanContinueIssuedOriginalTargetAddition(ProgramSkillFrame frame) => _resolve().CanContinueIssuedOriginalTargetAddition(frame);
    internal bool CanOfferOriginalTargetAddition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOriginalTargetAddition(candidate, trigger, context);
    internal void CollectIssuedOriginalTargetCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedOriginalTargetCandidates(action, window, result);
    internal int CompletedFangtianOriginalTargetIndex(NativeOwners.FangtianHalberdHandle multi) => _resolve().CompletedFangtianOriginalTargetIndex(multi);
    internal void ExpireOriginalTargetAdditionGrants(int turn, int seat) => _resolve().ExpireOriginalTargetAdditionGrants(turn, seat);
    internal void GrantOriginalTargetAddition(ProgramSkillFrame frame, int targetSeat) => _resolve().GrantOriginalTargetAddition(frame, targetSeat);
    internal bool HasIssuedOriginalTargetAdditionTail(CardUseFrame use) => _resolve().HasIssuedOriginalTargetAdditionTail(use);
    internal bool HasIssuedOriginalTargetArmorBypass(long id, int targetSeat) => _resolve().HasIssuedOriginalTargetArmorBypass(id, targetSeat);
    internal bool HasIssuedOriginalTargetCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedOriginalTargetCandidate(candidate, context);
    internal void IssueOriginalTargetAdditionPolicy(long id, CardActionContext? action) => _resolve().IssueOriginalTargetAdditionPolicy(id, action);
    internal SkillProgramStepOutcome OfferOriginalTargetAddition(ProgramSkillFrame frame) => _resolve().OfferOriginalTargetAddition(frame);
    internal IReadOnlyList<PromptChoice> OriginalTargetAdditionChoices(ProgramSkillFrame frame) => _resolve().OriginalTargetAdditionChoices(frame);
    internal bool OriginalTargetAdditionFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().OriginalTargetAdditionFinalizedInstantTrickRequirementTargetWindow(query);
    internal int[] OriginalTargetAdditionTargets(ProgramSkillFrame frame) => _resolve().OriginalTargetAdditionTargets(frame);
    internal void ResolveOriginalTargetAdditionChoice(PromptChoice choice) => _resolve().ResolveOriginalTargetAdditionChoice(choice);
    internal PromptChoice SelectAiOriginalTargetAddition(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiOriginalTargetAddition(decision, frame);
    internal bool TryGetOriginalTargetVirtualSlashReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primaryTarget) => _resolve().TryGetOriginalTargetVirtualSlashReturn(use, parent, out primaryTarget);
}
