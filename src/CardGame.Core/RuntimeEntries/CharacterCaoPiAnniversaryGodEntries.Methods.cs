#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiAnniversaryGodEntries
{
    internal global::CardGame.Core.KemingBorrowReceipt? ActiveKemingBorrow(int owner, string skill, string? instance = default) => _resolve().ActiveKemingBorrow(owner, skill, instance);
    internal void AnswerKemingBorrow(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerKemingBorrow(selected);
    internal void AnswerKemingEcho(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerKemingEcho(choice);
    internal void AssertKemingEcho(global::CardGame.Core.KemingSkillEchoFrame f) => _resolve().AssertKemingEcho(f);
    internal void AssertKemingSourceReceipt(global::CardGame.Core.KemingSourceCapture c) => _resolve().AssertKemingSourceReceipt(c);
    internal bool BeginKemingEcho(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.KemingSourceCapture capture, global::CardGame.Core.KemingEchoReturn returned) => _resolve().BeginKemingEcho(parent, capture, returned);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenCaoPi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenCaoPi(supplied, op);
    internal bool CanTriggerShenCaoPi(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanTriggerShenCaoPi(g);
    internal void CaptureKemingInvocation(long invocationId, long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string skillInstanceId) => _resolve().CaptureKemingInvocation(invocationId, parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId);
    internal global::CardGame.Core.PromptChoice ChooseKemingBorrowAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseKemingBorrowAi(p, f);
    internal void CollectYinzhiIssuedReward(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectYinzhiIssuedReward(action, window, result);
    internal void ContinueKemingSkillEcho(global::CardGame.Core.KemingSkillEchoFrame supplied) => _resolve().ContinueKemingSkillEcho(supplied);
    internal bool DrainShenCaoPi(long id) => _resolve().DrainShenCaoPi(id);
    internal void FinishShenCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenCaoPi(f);
    internal bool HasKemingBorrowedSkill(global::CardGame.Core.CharacterState owner, string skill) => _resolve().HasKemingBorrowedSkill(owner, skill);
    internal bool HasKemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill, string instance) => _resolve().HasKemingBorrowedSkillInstance(owner, skill, instance);
    internal bool HasKemingCompletedCardEffect(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().HasKemingCompletedCardEffect(action, window);
    internal bool HasKemingEchoPrompt() => _resolve().HasKemingEchoPrompt();
    internal bool IsIssuedShenCaoPiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedShenCaoPiCandidate(c, w);
    internal bool IsKemingEchoMovement(global::CardGame.Core.KemingSkillEchoFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsKemingEchoMovement(f, b);
    internal bool IsYinzhiSkillSuppressed(int seat) => _resolve().IsYinzhiSkillSuppressed(seat);
    internal bool IsYinzhiTurnRestricted(int actor) => _resolve().IsYinzhiTurnRestricted(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KemingBorrowChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KemingBorrowChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.KemingBorrowOption> KemingBorrowOptions(int borrower, int source) => _resolve().KemingBorrowOptions(borrower, source);
    internal string? KemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill) => _resolve().KemingBorrowedSkillInstance(owner, skill);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KemingEchoChoices(global::CardGame.Core.KemingSkillEchoFrame f) => _resolve().KemingEchoChoices(f);
    internal bool KemingEchoTypedParentMatches(global::CardGame.Core.KemingSkillEchoFrame f, global::CardGame.Core.ResolutionFrame parent) => _resolve().KemingEchoTypedParentMatches(f, parent);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.KemingBorrowOption> KemingPrintedBorrowOptions(global::CardGame.Core.ContentGeneralDefinition general) => _resolve().KemingPrintedBorrowOptions(general);
    internal bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().KemingSkillEchoFrameRidesOn(child, parent);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.KemingBorrowOption> KemingSourceBorrowOptions(int source) => _resolve().KemingSourceBorrowOptions(source);
    internal void ObserveKemingProducedFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveKemingProducedFacts(payload);
    internal void ObserveShenCaoPiRules(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShenCaoPiRules(payload);
    internal void PublishKemingBorrow(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishKemingBorrow(f);
    internal void PublishKemingEcho(global::CardGame.Core.KemingSkillEchoFrame f) => _resolve().PublishKemingEcho(f);
    internal bool ResumeShenCaoPi(long id) => _resolve().ResumeShenCaoPi(id);
    internal bool RetainShenCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainShenCaoPi(f);
    internal void ReturnKemingEchoMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window) => _resolve().ReturnKemingEchoMovement(window);
    internal void ReturnShenCaoPiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnShenCaoPiDraw(f, o);
    internal bool ReturnShenCaoPiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenCaoPiMovement(f);
    internal bool TryChooseKemingEchoAi(global::CardGame.Core.PendingDecision decision, scoped out global::CardGame.Core.PromptChoice selected) => _resolve().TryChooseKemingEchoAi(decision, out selected);
    internal bool TryCompleteKemingCixiong(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount) => _resolve().TryCompleteKemingCixiong(pending, activated, targetDiscarded, discardedCardId, sourceDrawCount);
    internal bool TryCompleteKemingConverted(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => _resolve().TryCompleteKemingConverted(supplied);
    internal bool TryCompleteKemingProgram(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().TryCompleteKemingProgram(f, completed);
    internal void ValidateYinzhiNewUse(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().ValidateYinzhiNewUse(q);
    internal global::CardGame.Core.CardActionContext? YinzhiAction(global::CardGame.Core.ProgramSkillWindowContext? c) => _resolve().YinzhiAction(c);
    internal bool YinzhiActionBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().YinzhiActionBan(q);
    internal bool YinzhiAwaitingOwnTurn(global::CardGame.Core.YinzhiIssuedEvent e) => _resolve().YinzhiAwaitingOwnTurn(e);
    internal bool YinzhiMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().YinzhiMaterialBan(q);
    internal bool YinzhiNewCardsForbidden(int actor, global::System.Collections.Generic.IEnumerable<int> ids) => _resolve().YinzhiNewCardsForbidden(actor, ids);
    internal int YinzhiOwnTurns(int seat) => _resolve().YinzhiOwnTurns(seat);
    internal bool YinzhiResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().YinzhiResponseBan(q);
    internal global::CardGame.Core.YinzhiIssuedEvent? YinzhiRewardIssue(int owner, global::CardGame.Core.CardActionContext? a) => _resolve().YinzhiRewardIssue(owner, a);
    internal bool HasKemingInvocationObservers { get => _resolve().HasKemingInvocationObservers; }
    internal bool HasShenCaoPiCapability { get => _resolve().HasShenCaoPiCapability; }
}
