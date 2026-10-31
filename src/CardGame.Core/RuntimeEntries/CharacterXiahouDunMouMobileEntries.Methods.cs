#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunMouMobileEntries
{
    internal bool AdvanceMouXiahouDunChildren() => _resolve().AdvanceMouXiahouDunChildren();
    internal PromptChoice AiMouXiahouDun(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouXiahouDun(p, f);
    internal void AnswerMouXiahouDun(PromptChoice selected) => _resolve().AnswerMouXiahouDun(selected);
    internal bool CanOfferMouXiahouDunQingjian(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouXiahouDunQingjian(gate);
    internal bool CanStartMouXiahouDunGanglie(CharacterState owner) => _resolve().CanStartMouXiahouDunGanglie(owner);
    internal CardMovementBatchContext CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(batch, completed);
    internal bool ContinueAcceptedMouXiahouDun(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouXiahouDun(f);
    internal void ContinueMouXiahouDunMovement(long id) => _resolve().ContinueMouXiahouDunMovement(id);
    internal void FinishMouXiahouDun(ProgramSkillFrame f) => _resolve().FinishMouXiahouDun(f);
    internal bool IsIssuedMouXiahouDunPile(PublicPersistentPileSource source) => _resolve().IsIssuedMouXiahouDunPile(source);
    internal bool IsMouXiahouDunDying() => _resolve().IsMouXiahouDunDying();
    internal bool IsMouXiahouDunMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouXiahouDunMovement(f, e, p);
    internal bool IsMouXiahouDunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouXiahouDunRecoveryParent(f, recovery);
    internal void IssueMouXiahouDunDamage(ProgramSkillFrame f) => _resolve().IssueMouXiahouDunDamage(f);
    internal int[] MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch) => _resolve().MatchingMouXiahouDunDiscardIndexes(batch);
    internal PromptChoice MouXiahouDunChoice(ProgramSkillFrame f, string branch, string label, int? target = default, int? card = default) => _resolve().MouXiahouDunChoice(f, branch, label, target, card);
    internal IReadOnlyList<PromptChoice> MouXiahouDunChoices(ProgramSkillFrame f) => _resolve().MouXiahouDunChoices(f);
    internal bool MouXiahouDunFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouXiahouDunFirstChild(f, child);
    internal int[] MouXiahouDunGanglieTargets(int seat) => _resolve().MouXiahouDunGanglieTargets(seat);
    internal bool MouXiahouDunObserverEdge(int n) => _resolve().MouXiahouDunObserverEdge(n);
    internal ProgramSkillFrame? MouXiahouDunObserverRoot() => _resolve().MouXiahouDunObserverRoot();
    internal bool MouXiahouDunObserverSuffix(int i) => _resolve().MouXiahouDunObserverSuffix(i);
    internal int MouXiahouDunPhaseUses(int seat, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouXiahouDunPhaseUses(seat, phase);
    internal PublicPersistentPileSource? MouXiahouDunPile(int seat, string instance) => _resolve().MouXiahouDunPile(seat, instance);
    internal int MouXiahouDunProjectedUses(int seat, string skill, int fallback) => _resolve().MouXiahouDunProjectedUses(seat, skill, fallback);
    internal int MouXiahouDunQingjianCapacity(int seat) => _resolve().MouXiahouDunQingjianCapacity(seat);
    internal int MouXiahouDunQingjianCount(int seat) => _resolve().MouXiahouDunQingjianCount(seat);
    internal void PublishMouXiahouDun(ProgramSkillFrame f) => _resolve().PublishMouXiahouDun(f);
    internal void RecordMouXiahouDunDiscardOrigins(CardMovementBatchContext batch) => _resolve().RecordMouXiahouDunDiscardOrigins(batch);
    internal bool ResumeMouXiahouDun(long id) => _resolve().ResumeMouXiahouDun(id);
    internal bool ReturnMouXiahouDunMovement(ProgramSkillFrame f) => _resolve().ReturnMouXiahouDunMovement(f);
    internal void SealMouXiahouDunMovements(long id, long before) => _resolve().SealMouXiahouDunMovements(id, before);
    internal SkillProgramStepOutcome StartMouXiahouDunMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().StartMouXiahouDunMobile(supplied, op);
    internal bool ValidMouXiahouDun(ProgramSkillFrame f) => _resolve().ValidMouXiahouDun(f);
    internal bool HasMouXiahouDunCapability { get => _resolve().HasMouXiahouDunCapability; }
}
