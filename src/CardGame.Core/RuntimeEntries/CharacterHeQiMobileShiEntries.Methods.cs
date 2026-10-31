#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileShiEntries
{
    internal bool AdvanceShiHqChildren() => _resolve().AdvanceShiHqChildren();
    internal void AnswerShiHq(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiHq(choice);
    internal bool BanShiHqRepeatedTarget(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().BanShiHqRepeatedTarget(q);
    internal bool BanShiHqUnmarkedConversion(global::CardGame.Core.ProgramViewAsSourceQuery q) => _resolve().BanShiHqUnmarkedConversion(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiHq(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiHq(supplied, op);
    internal bool CanOfferShiHq(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferShiHq(c, t, w);
    internal global::CardGame.Core.PromptChoice ChooseShiHqAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiHqAi(p, f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectShiHqMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectShiHqMovementCandidates(batch);
    internal void CollectWindow_CollectShiHqMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectShiHqMovementCandidates(batch, result);
    internal void ContinueShiHq(long id) => _resolve().ContinueShiHq(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateShiHqMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateShiHqMovementContext(w, c);
    internal bool ExcludeOrdinaryMovement_IsShiHqMarkTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsShiHqMarkTrigger(candidate, trigger);
    internal void FinishShiHq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiHq(f);
    internal bool HasShiHqYingji(int id) => _resolve().HasShiHqYingji(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeShiHqAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeShiHqAfterTurnEndedItems(parent, ordinary);
    internal bool IsCurrentShiHqFieldEntry(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsCurrentShiHqFieldEntry(c, context);
    internal bool IsIssuedShiHqCleanupCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedShiHqCleanupCandidate(c, w);
    internal bool IsShiHqOriginalHandPayment(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action, int actor, int cardId, global::CardGame.Core.CardLocation from) => _resolve().IsShiHqOriginalHandPayment(use, action, actor, cardId, from);
    internal bool KeepPaidShiHq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShiHq(f);
    internal void MarkShiHqEntity(global::CardGame.Core.ProgramSkillFrame f, int id, int holder, global::CardGame.Core.ShiHqMarkOrigin origin) => _resolve().MarkShiHqEntity(f, id, holder, origin);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiHeQiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiHeQiPublicStates(owner, skillId);
    internal void ObserveShiHqDuel(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShiHqDuel(payload);
    internal bool? ProveShiHqMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveShiHqMovement(trigger, candidate, context);
    internal void PublishShiHq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiHq(f);
    internal void RequireShiHq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShiHq(f);
    internal bool ResumeShiHq(long id) => _resolve().ResumeShiHq(id);
    internal void ReturnShiHqDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiHqDraw(f, outcome);
    internal bool ReturnShiHqMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiHqMovement(f);
    internal bool ShiHqChildEdge(int i) => _resolve().ShiHqChildEdge(i);
    internal bool ShiHqChildSuffix(int n) => _resolve().ShiHqChildSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiHqChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiHqChoices(f);
    internal global::CardGame.Core.ShiHqYingjiMarkedEvent? ShiHqCleanupSource() => _resolve().ShiHqCleanupSource();
    internal bool ShiHqDyingChild() => _resolve().ShiHqDyingChild();
    internal bool ShiHqFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiHqFirstChild(f, child);
    internal global::CardGame.Core.ShiHqYingjiMarkedEvent? ShiHqMark(int id) => _resolve().ShiHqMark(id);
    internal bool ShiHqMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiHqMovementProof(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiHqObserverRoot() => _resolve().ShiHqObserverRoot();
    internal bool ShiHqRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiHqRecoveryParent(f, recovery);
    internal bool ShiHqTargetUsed(int owner, int target) => _resolve().ShiHqTargetUsed(owner, target);
    internal string? ShiHqYingjiLabel(int id) => _resolve().ShiHqYingjiLabel(id);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateShiHqMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateShiHqMovementContext(frame, candidate);
    internal bool HasShiHqCapability { get => _resolve().HasShiHqCapability; }
}
