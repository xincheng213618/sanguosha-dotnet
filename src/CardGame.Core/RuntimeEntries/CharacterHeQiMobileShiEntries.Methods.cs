#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileShiEntries
{
    internal bool AdvanceShiHqChildren() => _resolve().AdvanceShiHqChildren();
    internal void AnswerShiHq(PromptChoice choice) => _resolve().AnswerShiHq(choice);
    internal bool BanShiHqRepeatedTarget(ProgramLegalActionQuery q) => _resolve().BanShiHqRepeatedTarget(q);
    internal bool BanShiHqUnmarkedConversion(ProgramViewAsSourceQuery q) => _resolve().BanShiHqUnmarkedConversion(q);
    internal SkillProgramStepOutcome BeginShiHq(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShiHq(supplied, op);
    internal bool CanOfferShiHq(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferShiHq(c, t, w);
    internal PromptChoice ChooseShiHqAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiHqAi(p, f);
    internal IEnumerable<ProgramTriggerCandidate> CollectShiHqMovementCandidates(CardMovementBatchContext batch) => _resolve().CollectShiHqMovementCandidates(batch);
    internal void CollectWindow_CollectShiHqMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectShiHqMovementCandidates(batch, result);
    internal void ContinueShiHq(long id) => _resolve().ContinueShiHq(id);
    internal ProgramSkillWindowContext CreateShiHqMovementContext(CardsMovedTriggerWindowFrame w, ProgramTriggerCandidate c) => _resolve().CreateShiHqMovementContext(w, c);
    internal bool ExcludeOrdinaryMovement_IsShiHqMarkTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsShiHqMarkTrigger(candidate, trigger);
    internal void FinishShiHq(ProgramSkillFrame f) => _resolve().FinishShiHq(f);
    internal bool HasShiHqYingji(int id) => _resolve().HasShiHqYingji(id);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeShiHqAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeShiHqAfterTurnEndedItems(parent, ordinary);
    internal bool IsCurrentShiHqFieldEntry(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsCurrentShiHqFieldEntry(c, context);
    internal bool IsIssuedShiHqCleanupCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedShiHqCleanupCandidate(c, w);
    internal bool IsShiHqOriginalHandPayment(CardUseFrame use, CardActionContext action, int actor, int cardId, CardLocation from) => _resolve().IsShiHqOriginalHandPayment(use, action, actor, cardId, from);
    internal bool KeepPaidShiHq(ProgramSkillFrame f) => _resolve().KeepPaidShiHq(f);
    internal void MarkShiHqEntity(ProgramSkillFrame f, int id, int holder, ShiHqMarkOrigin origin) => _resolve().MarkShiHqEntity(f, id, holder, origin);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiHeQiPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiHeQiPublicStates(owner, skillId);
    internal void ObserveShiHqDuel(IGameEvent payload) => _resolve().ObserveShiHqDuel(payload);
    internal bool? ProveShiHqMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveShiHqMovement(trigger, candidate, context);
    internal void PublishShiHq(ProgramSkillFrame f) => _resolve().PublishShiHq(f);
    internal void RequireShiHq(ProgramSkillFrame f) => _resolve().RequireShiHq(f);
    internal bool ResumeShiHq(long id) => _resolve().ResumeShiHq(id);
    internal void ReturnShiHqDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiHqDraw(f, outcome);
    internal bool ReturnShiHqMovement(ProgramSkillFrame f) => _resolve().ReturnShiHqMovement(f);
    internal bool ShiHqChildEdge(int i) => _resolve().ShiHqChildEdge(i);
    internal bool ShiHqChildSuffix(int n) => _resolve().ShiHqChildSuffix(n);
    internal IReadOnlyList<PromptChoice> ShiHqChoices(ProgramSkillFrame f) => _resolve().ShiHqChoices(f);
    internal ShiHqYingjiMarkedEvent? ShiHqCleanupSource() => _resolve().ShiHqCleanupSource();
    internal bool ShiHqDyingChild() => _resolve().ShiHqDyingChild();
    internal bool ShiHqFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiHqFirstChild(f, child);
    internal ShiHqYingjiMarkedEvent? ShiHqMark(int id) => _resolve().ShiHqMark(id);
    internal bool ShiHqMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().ShiHqMovementProof(f, effect, p);
    internal ProgramSkillFrame? ShiHqObserverRoot() => _resolve().ShiHqObserverRoot();
    internal bool ShiHqRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiHqRecoveryParent(f, recovery);
    internal bool ShiHqTargetUsed(int owner, int target) => _resolve().ShiHqTargetUsed(owner, target);
    internal string? ShiHqYingjiLabel(int id) => _resolve().ShiHqYingjiLabel(id);
    internal ProgramSkillWindowContext? TryCreateShiHqMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateShiHqMovementContext(frame, candidate);
    internal bool HasShiHqCapability { get => _resolve().HasShiHqCapability; }
}
