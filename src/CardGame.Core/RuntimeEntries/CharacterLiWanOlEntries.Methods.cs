#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiWanOlEntries
{
    internal bool AdvanceLiWanSubtree() => _resolve().AdvanceLiWanSubtree();
    internal global::CardGame.Core.PromptChoice AiLiWan(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiLiWan(prompt, f);
    internal void AnswerLiWan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerLiWan(choice);
    internal void AppendLianjuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendLianjuEndingItems(items);
    internal void AssertLiWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLiWan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiWan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLiWan(supplied, op);
    internal bool CanOfferLiWan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiWan(c, t, context);
    internal bool CanRunIssuedLiWanCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIssuedLiWanCandidate(c, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectSilvRefundCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectSilvRefundCandidates(batch);
    internal void CollectWindow_CollectSilvRefundCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectSilvRefundCandidates(batch, result);
    internal bool ContinueIssuedLiWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedLiWan(f);
    internal bool? DecideIssuedTrigger550(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger550(candidate, context);
    internal bool DrainLiWanChildren(long id) => _resolve().DrainLiWanChildren(id);
    internal void FreezeLianjuActorChange(global::CardGame.Core.IGameEvent fact) => _resolve().FreezeLianjuActorChange(fact);
    internal bool HasLiWanBeforeDamageObserver(long id) => _resolve().HasLiWanBeforeDamageObserver(id);
    internal bool HasLiWanDamageObserver(long id) => _resolve().HasLiWanDamageObserver(id);
    internal bool HasSilvEntityMark(int cardId) => _resolve().HasSilvEntityMark(cardId);
    internal bool IsIssuedLiWanCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsIssuedLiWanCandidate(c);
    internal bool IsLiWanDying() => _resolve().IsLiWanDying();
    internal bool IsLiWanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsLiWanMovement(f, effect, pending);
    internal bool IsLiWanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLiWanRecoveryParent(f, recovery);
    internal bool IsRefundableSilvMovement(global::CardGame.Core.CardMovementRecord m) => _resolve().IsRefundableSilvMovement(m);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiWanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiWanChoices(f);
    internal bool LiWanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiWanFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiWanObserverRoot() => _resolve().LiWanObserverRoot();
    internal global::CardGame.Core.LianjuPromiseGrantedEvent[] LianjuDuePromises() => _resolve().LianjuDuePromises();
    internal global::System.Collections.Generic.IReadOnlyList<int> LianjuEligibleCards(int actor, bool red) => _resolve().LianjuEligibleCards(actor, red);
    internal int LianjuFinalUseActor(global::CardGame.Core.LianjuEntityUsedEvent origin) => _resolve().LianjuFinalUseActor(origin);
    internal void ObserveLiWanActualUseAndEnding(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveLiWanActualUseAndEnding(fact);
    internal void PayLiWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayLiWan(f);
    internal void ProjectSilvCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectSilvCardSkillMark(cardId, ref projection);
    internal void PublishLiWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLiWan(f);
    internal void RecordLianjuEntityUse(int actor, long? action, long nativeUse, int card) => _resolve().RecordLianjuEntityUse(actor, action, nativeUse, card);
    internal bool ResumeLiWan(long id) => _resolve().ResumeLiWan(id);
    internal bool ReturnLiWanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiWanMovement(f);
    internal global::CardGame.Core.SilvEntityMarkedEvent? SilvMark(int id) => _resolve().SilvMark(id);
    internal bool ValidSilvMark(global::CardGame.Core.SilvEntityMarkedEvent e) => _resolve().ValidSilvMark(e);
    internal bool HasLiWanCapability { get => _resolve().HasLiWanCapability; }
    internal int LianjuEndingSerial { get => _resolve().LianjuEndingSerial; }
}
