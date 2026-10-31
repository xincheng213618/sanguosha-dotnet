#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiWanOlEntries
{
    internal bool AdvanceLiWanSubtree() => _resolve().AdvanceLiWanSubtree();
    internal PromptChoice AiLiWan(PendingDecision prompt, ProgramSkillFrame f) => _resolve().AiLiWan(prompt, f);
    internal void AnswerLiWan(PromptChoice choice) => _resolve().AnswerLiWan(choice);
    internal void AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendLianjuEndingItems(items);
    internal SkillProgramStepOutcome BeginLiWan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLiWan(supplied, op);
    internal bool CanOfferLiWan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferLiWan(c, t, context);
    internal bool CanRunIssuedLiWanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunIssuedLiWanCandidate(c, context);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectSilvRefundCandidates(CardMovementBatchContext batch) => _resolve().CollectSilvRefundCandidates(batch);
    internal void CollectWindow_CollectSilvRefundCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectSilvRefundCandidates(batch, result);
    internal bool ContinueIssuedLiWan(ProgramSkillFrame f) => _resolve().ContinueIssuedLiWan(f);
    internal bool? DecideIssuedTrigger550(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger550(candidate, context);
    internal bool DrainLiWanChildren(long id) => _resolve().DrainLiWanChildren(id);
    internal void FreezeLianjuActorChange(IGameEvent fact) => _resolve().FreezeLianjuActorChange(fact);
    internal bool HasSilvEntityMark(int cardId) => _resolve().HasSilvEntityMark(cardId);
    internal bool IsIssuedLiWanCandidate(ProgramTriggerCandidate c) => _resolve().IsIssuedLiWanCandidate(c);
    internal bool IsLiWanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiWanRecoveryParent(f, recovery);
    internal bool IsRefundableSilvMovement(CardMovementRecord m) => _resolve().IsRefundableSilvMovement(m);
    internal IReadOnlyList<PromptChoice> LiWanChoices(ProgramSkillFrame f) => _resolve().LiWanChoices(f);
    internal bool LiWanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiWanFirstChild(f, child);
    internal ProgramSkillFrame? LiWanObserverRoot() => _resolve().LiWanObserverRoot();
    internal LianjuPromiseGrantedEvent[] LianjuDuePromises() => _resolve().LianjuDuePromises();
    internal IReadOnlyList<int> LianjuEligibleCards(int actor, bool red) => _resolve().LianjuEligibleCards(actor, red);
    internal int LianjuFinalUseActor(LianjuEntityUsedEvent origin) => _resolve().LianjuFinalUseActor(origin);
    internal void ObserveLiWanActualUseAndEnding(IGameEvent fact) => _resolve().ObserveLiWanActualUseAndEnding(fact);
    internal void PayLiWan(ProgramSkillFrame f) => _resolve().PayLiWan(f);
    internal void ProjectSilvCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectSilvCardSkillMark(cardId, ref projection);
    internal void PublishLiWan(ProgramSkillFrame f) => _resolve().PublishLiWan(f);
    internal void RecordLianjuEntityUse(int actor, long? action, long nativeUse, int card) => _resolve().RecordLianjuEntityUse(actor, action, nativeUse, card);
    internal bool ResumeLiWan(long id) => _resolve().ResumeLiWan(id);
    internal bool ReturnLiWanMovement(ProgramSkillFrame f) => _resolve().ReturnLiWanMovement(f);
    internal SilvEntityMarkedEvent? SilvMark(int id) => _resolve().SilvMark(id);
    internal bool ValidSilvMark(SilvEntityMarkedEvent e) => _resolve().ValidSilvMark(e);
    internal bool HasLiWanCapability { get => _resolve().HasLiWanCapability; }
    internal int LianjuEndingSerial { get => _resolve().LianjuEndingSerial; }
}
