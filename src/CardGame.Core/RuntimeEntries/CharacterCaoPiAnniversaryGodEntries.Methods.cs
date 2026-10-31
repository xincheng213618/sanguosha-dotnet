#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiAnniversaryGodEntries
{
    internal void AnswerKemingBorrow(PromptChoice selected) => _resolve().AnswerKemingBorrow(selected);
    internal void AnswerKemingEcho(PromptChoice choice) => _resolve().AnswerKemingEcho(choice);
    internal void AssertKemingEcho(KemingSkillEchoFrame f) => _resolve().AssertKemingEcho(f);
    internal void AssertKemingSourceReceipt(KemingSourceCapture c) => _resolve().AssertKemingSourceReceipt(c);
    internal bool BeginKemingEcho(ResolutionFrame parent, KemingSourceCapture capture, KemingEchoReturn returned) => _resolve().BeginKemingEcho(parent, capture, returned);
    internal SkillProgramStepOutcome BeginShenCaoPi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenCaoPi(supplied, op);
    internal bool CanTriggerShenCaoPi(ProgramTriggerLegalityContext g) => _resolve().CanTriggerShenCaoPi(g);
    internal void CaptureKemingInvocation(long invocationId, long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string skillInstanceId) => _resolve().CaptureKemingInvocation(invocationId, parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId);
    internal PromptChoice ChooseKemingBorrowAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseKemingBorrowAi(p, f);
    internal void CollectYinzhiIssuedReward(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectYinzhiIssuedReward(action, window, result);
    internal void ContinueKemingSkillEcho(KemingSkillEchoFrame supplied) => _resolve().ContinueKemingSkillEcho(supplied);
    internal bool DrainShenCaoPi(long id) => _resolve().DrainShenCaoPi(id);
    internal void FinishShenCaoPi(ProgramSkillFrame f) => _resolve().FinishShenCaoPi(f);
    internal bool HasKemingCompletedCardEffect(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().HasKemingCompletedCardEffect(action, window);
    internal bool HasKemingEchoPrompt() => _resolve().HasKemingEchoPrompt();
    internal bool IsIssuedShenCaoPiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedShenCaoPiCandidate(c, w);
    internal bool IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b) => _resolve().IsKemingEchoMovement(f, b);
    internal bool IsYinzhiTurnRestricted(int actor) => _resolve().IsYinzhiTurnRestricted(actor);
    internal IReadOnlyList<PromptChoice> KemingBorrowChoices(ProgramSkillFrame f) => _resolve().KemingBorrowChoices(f);
    internal IReadOnlyList<KemingBorrowOption> KemingBorrowOptions(int borrower, int source) => _resolve().KemingBorrowOptions(borrower, source);
    internal IReadOnlyList<PromptChoice> KemingEchoChoices(KemingSkillEchoFrame f) => _resolve().KemingEchoChoices(f);
    internal bool KemingEchoTypedParentMatches(KemingSkillEchoFrame f, ResolutionFrame parent) => _resolve().KemingEchoTypedParentMatches(f, parent);
    internal IReadOnlyList<KemingBorrowOption> KemingPrintedBorrowOptions(ContentGeneralDefinition general) => _resolve().KemingPrintedBorrowOptions(general);
    internal bool KemingSkillEchoFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().KemingSkillEchoFrameRidesOn(child, parent);
    internal IReadOnlyList<KemingBorrowOption> KemingSourceBorrowOptions(int source) => _resolve().KemingSourceBorrowOptions(source);
    internal void ObserveKemingProducedFacts(IGameEvent payload) => _resolve().ObserveKemingProducedFacts(payload);
    internal void ObserveShenCaoPiRules(IGameEvent payload) => _resolve().ObserveShenCaoPiRules(payload);
    internal void PublishKemingBorrow(ProgramSkillFrame f) => _resolve().PublishKemingBorrow(f);
    internal void PublishKemingEcho(KemingSkillEchoFrame f) => _resolve().PublishKemingEcho(f);
    internal bool ResumeShenCaoPi(long id) => _resolve().ResumeShenCaoPi(id);
    internal bool RetainShenCaoPi(ProgramSkillFrame f) => _resolve().RetainShenCaoPi(f);
    internal void ReturnKemingEchoMovement(CardsMovedTriggerWindowFrame window) => _resolve().ReturnKemingEchoMovement(window);
    internal void ReturnShenCaoPiDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnShenCaoPiDraw(f, o);
    internal bool ReturnShenCaoPiMovement(ProgramSkillFrame f) => _resolve().ReturnShenCaoPiMovement(f);
    internal bool TryChooseKemingEchoAi(PendingDecision decision, scoped out PromptChoice selected) => _resolve().TryChooseKemingEchoAi(decision, out selected);
    internal bool TryCompleteKemingCixiong(NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount) => _resolve().TryCompleteKemingCixiong(pending, activated, targetDiscarded, discardedCardId, sourceDrawCount);
    internal bool TryCompleteKemingConverted(ProgramCardTriggerWindowFrame supplied) => _resolve().TryCompleteKemingConverted(supplied);
    internal bool TryCompleteKemingProgram(ProgramSkillFrame f, bool completed) => _resolve().TryCompleteKemingProgram(f, completed);
    internal void ValidateYinzhiNewUse(ProgramSubmittedUseQuery q) => _resolve().ValidateYinzhiNewUse(q);
    internal CardActionContext? YinzhiAction(ProgramSkillWindowContext? c) => _resolve().YinzhiAction(c);
    internal bool YinzhiActionBan(ProgramLegalActionQuery q) => _resolve().YinzhiActionBan(q);
    internal bool YinzhiAwaitingOwnTurn(YinzhiIssuedEvent e) => _resolve().YinzhiAwaitingOwnTurn(e);
    internal bool YinzhiMaterialBan(ProgramCardMaterialQuery q) => _resolve().YinzhiMaterialBan(q);
    internal bool YinzhiNewCardsForbidden(int actor, IEnumerable<int> ids) => _resolve().YinzhiNewCardsForbidden(actor, ids);
    internal int YinzhiOwnTurns(int seat) => _resolve().YinzhiOwnTurns(seat);
    internal bool YinzhiResponseBan(ProgramNativeResponseQuery q) => _resolve().YinzhiResponseBan(q);
    internal YinzhiIssuedEvent? YinzhiRewardIssue(int owner, CardActionContext? a) => _resolve().YinzhiRewardIssue(owner, a);
    internal bool HasKemingInvocationObservers { get => _resolve().HasKemingInvocationObservers; }
    internal bool HasShenCaoPiCapability { get => _resolve().HasShenCaoPiCapability; }
}
