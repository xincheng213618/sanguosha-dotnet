#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangWumiaoEntries
{
    internal bool AdvanceWuZglChildren() => _resolve().AdvanceWuZglChildren();
    internal PromptChoice AiWuZgl(PendingDecision p, ProgramSkillFrame f) => _resolve().AiWuZgl(p, f);
    internal void AnswerWuZgl(PromptChoice selected) => _resolve().AnswerWuZgl(selected);
    internal SkillProgramStepOutcome BeginWuZgl(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginWuZgl(supplied, op);
    internal bool CanOfferWuZgl(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferWuZgl(candidate, t, c);
    internal bool CanWuZglQingshi(int owner, ProgramSkillWindowContext c) => _resolve().CanWuZglQingshi(owner, c);
    internal bool ContinuePaidWuZgl(ProgramSkillFrame f) => _resolve().ContinuePaidWuZgl(f);
    internal void ContinueWuZgl(long id) => _resolve().ContinueWuZgl(id);
    internal bool DrainWuZgl(ProgramSkillFrame f) => _resolve().DrainWuZgl(f);
    internal void FinishWuZgl(ProgramSkillFrame f) => _resolve().FinishWuZgl(f);
    internal bool IsWuZglCopyBlocked(int actor, int id) => _resolve().IsWuZglCopyBlocked(actor, id);
    internal bool IsWuZglDying() => _resolve().IsWuZglDying();
    internal bool IsWuZglRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame r) => _resolve().IsWuZglRecoveryParent(f, r);
    internal bool IsWuZglReturnedCopyCost(CardActionContext action, CardActionCost cost, CardMovementRecord? finish = default) => _resolve().IsWuZglReturnedCopyCost(action, cost, finish);
    internal bool IsWuZglUseMaterialForbidden(int actor, IReadOnlyList<int> ids, long? existingUseId = default) => _resolve().IsWuZglUseMaterialForbidden(actor, ids, existingUseId);
    internal CardLocation NormalizeWuZglCopyDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _resolve().NormalizeWuZglCopyDestination(card, from, to, reason);
    internal void ProjectWuZhugeLiangCopyCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectWuZhugeLiangCopyCardSkillMark(cardId, ref projection);
    internal void PublishWuZgl(ProgramSkillFrame f) => _resolve().PublishWuZgl(f);
    internal void RecordWuZglCopyMovement(CardMovementRecord move) => _resolve().RecordWuZglCopyMovement(move);
    internal bool ResumeWuZgl(long id) => _resolve().ResumeWuZgl(id);
    internal void ReturnWuZglDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnWuZglDraw(f, o);
    internal bool ReturnWuZglMovement(ProgramSkillFrame f) => _resolve().ReturnWuZglMovement(f);
    internal void SealWuZgl(long id, long before) => _resolve().SealWuZgl(id, before);
    internal void StartWuZglDraw(ProgramSkillFrame f, int recipient, int count) => _resolve().StartWuZglDraw(f, recipient, count);
    internal bool ValidWuZgl(ProgramSkillFrame f) => _resolve().ValidWuZgl(f);
    internal CardActionContext? WuZglAction(ProgramSkillWindowContext c) => _resolve().WuZglAction(c);
    internal PromptChoice WuZglChoice(ProgramSkillFrame f, string key, string label, int? target = default) => _resolve().WuZglChoice(f, key, label, target);
    internal IReadOnlyList<PromptChoice> WuZglChoices(ProgramSkillFrame f) => _resolve().WuZglChoices(f);
    internal WuZhugeLiangCopyCreatedEvent? WuZglCopyFact(int id) => _resolve().WuZglCopyFact(id);
    internal string? WuZglCopyLabel(int id) => _resolve().WuZglCopyLabel(id);
    internal CardMovementRecord? WuZglCopyReturnEntry(Card card, CardLocation from, CardMoveReason reason) => _resolve().WuZglCopyReturnEntry(card, from, reason);
    internal bool WuZglFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WuZglFirstChild(f, child);
    internal ProgramSkillFrame? WuZglObserverRoot() => _resolve().WuZglObserverRoot();
    internal bool WuZglObserverSuffix(int i) => _resolve().WuZglObserverSuffix(i);
    internal CardSnapshot[] WuZglPrivateTopCards(int viewer) => _resolve().WuZglPrivateTopCards(viewer);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> WuZglQingshiDamage(GameEngine.IDamageAttempt attack) => _resolve().WuZglQingshiDamage(attack);
    internal bool WuZglTurnOpen(int turn, int owner) => _resolve().WuZglTurnOpen(turn, owner);
    internal bool HasWuZhugeLiangCapability { get => _resolve().HasWuZhugeLiangCapability; }
}
