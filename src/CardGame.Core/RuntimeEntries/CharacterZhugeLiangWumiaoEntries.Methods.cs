#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangWumiaoEntries
{
    internal bool AdvanceWuZglChildren() => _resolve().AdvanceWuZglChildren();
    internal global::CardGame.Core.PromptChoice AiWuZgl(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiWuZgl(p, f);
    internal void AnswerWuZgl(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerWuZgl(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWuZgl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWuZgl(supplied, op);
    internal bool CanOfferWuZgl(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferWuZgl(candidate, t, c);
    internal bool CanWuZglQingshi(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanWuZglQingshi(owner, c);
    internal bool ContinuePaidWuZgl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWuZgl(f);
    internal void ContinueWuZgl(long id) => _resolve().ContinueWuZgl(id);
    internal bool DrainWuZgl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainWuZgl(f);
    internal void FinishWuZgl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWuZgl(f);
    internal bool HasWuZglBeforeDamageObserver(long id) => _resolve().HasWuZglBeforeDamageObserver(id);
    internal bool HasWuZglDamageObserver(long id) => _resolve().HasWuZglDamageObserver(id);
    internal bool IsWuZglCopyBlocked(int actor, int id) => _resolve().IsWuZglCopyBlocked(actor, id);
    internal bool IsWuZglDying() => _resolve().IsWuZglDying();
    internal bool IsWuZglMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsWuZglMovement(f, effect, pending);
    internal bool IsWuZglRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame r) => _resolve().IsWuZglRecoveryParent(f, r);
    internal bool IsWuZglReturnedCopyCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord? finish = default) => _resolve().IsWuZglReturnedCopyCost(action, cost, finish);
    internal bool IsWuZglUseMaterialForbidden(int actor, global::System.Collections.Generic.IReadOnlyList<int> ids, long? existingUseId = default) => _resolve().IsWuZglUseMaterialForbidden(actor, ids, existingUseId);
    internal global::CardGame.Core.CardLocation NormalizeWuZglCopyDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => _resolve().NormalizeWuZglCopyDestination(card, from, to, reason);
    internal void ProjectWuZhugeLiangCopyCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectWuZhugeLiangCopyCardSkillMark(cardId, ref projection);
    internal void PublishWuZgl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWuZgl(f);
    internal void RecordWuZglCopyMovement(global::CardGame.Core.CardMovementRecord move) => _resolve().RecordWuZglCopyMovement(move);
    internal bool ResumeWuZgl(long id) => _resolve().ResumeWuZgl(id);
    internal void ReturnWuZglDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnWuZglDraw(f, o);
    internal bool ReturnWuZglMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWuZglMovement(f);
    internal void SealWuZgl(long id, long before) => _resolve().SealWuZgl(id, before);
    internal void StartWuZglDraw(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count) => _resolve().StartWuZglDraw(f, recipient, count);
    internal bool ValidWuZgl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWuZgl(f);
    internal global::CardGame.Core.CardActionContext? WuZglAction(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WuZglAction(c);
    internal global::CardGame.Core.PromptChoice WuZglChoice(global::CardGame.Core.ProgramSkillFrame f, string key, string label, int? target = default) => _resolve().WuZglChoice(f, key, label, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WuZglChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WuZglChoices(f);
    internal global::CardGame.Core.WuZhugeLiangCopyCreatedEvent? WuZglCopyFact(int id) => _resolve().WuZglCopyFact(id);
    internal string? WuZglCopyLabel(int id) => _resolve().WuZglCopyLabel(id);
    internal global::CardGame.Core.CardMovementRecord? WuZglCopyReturnEntry(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => _resolve().WuZglCopyReturnEntry(card, from, reason);
    internal bool WuZglFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WuZglFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? WuZglObserverRoot() => _resolve().WuZglObserverRoot();
    internal bool WuZglObserverSuffix(int i) => _resolve().WuZglObserverSuffix(i);
    internal global::CardGame.Core.CardSnapshot[] WuZglPrivateTopCards(int viewer) => _resolve().WuZglPrivateTopCards(viewer);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> WuZglQingshiDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WuZglQingshiDamage(attack);
    internal bool WuZglTurnOpen(int turn, int owner) => _resolve().WuZglTurnOpen(turn, owner);
    internal bool HasWuZhugeLiangCapability { get => _resolve().HasWuZhugeLiangCapability; }
}
