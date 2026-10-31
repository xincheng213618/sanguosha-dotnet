#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunMouMobileEntries
{
    internal bool AdvanceMouXiahouDunChildren() => _resolve().AdvanceMouXiahouDunChildren();
    internal global::CardGame.Core.PromptChoice AiMouXiahouDun(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouXiahouDun(p, f);
    internal void AnswerMouXiahouDun(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouXiahouDun(selected);
    internal bool CanOfferMouXiahouDunQingjian(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouXiahouDunQingjian(gate);
    internal bool CanStartMouXiahouDunGanglie(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouXiahouDunGanglie(owner);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(batch, completed);
    internal bool ContinueAcceptedMouXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouXiahouDun(f);
    internal void ContinueMouXiahouDunMovement(long id) => _resolve().ContinueMouXiahouDunMovement(id);
    internal void FinishMouXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouXiahouDun(f);
    internal bool IsIssuedMouXiahouDunPile(global::CardGame.Core.PublicPersistentPileSource source) => _resolve().IsIssuedMouXiahouDunPile(source);
    internal bool IsMouXiahouDunDying() => _resolve().IsMouXiahouDunDying();
    internal bool IsMouXiahouDunMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouXiahouDunMovement(f, e, p);
    internal bool IsMouXiahouDunRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouXiahouDunRecoveryParent(f, recovery);
    internal void IssueMouXiahouDunDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouXiahouDunDamage(f);
    internal int[] MatchingMouXiahouDunDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MatchingMouXiahouDunDiscardIndexes(batch);
    internal global::CardGame.Core.PromptChoice MouXiahouDunChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? target = default, int? card = default) => _resolve().MouXiahouDunChoice(f, branch, label, target, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXiahouDunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXiahouDunChoices(f);
    internal bool MouXiahouDunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouXiahouDunFirstChild(f, child);
    internal int[] MouXiahouDunGanglieTargets(int seat) => _resolve().MouXiahouDunGanglieTargets(seat);
    internal bool MouXiahouDunObserverEdge(int n) => _resolve().MouXiahouDunObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouXiahouDunObserverRoot() => _resolve().MouXiahouDunObserverRoot();
    internal bool MouXiahouDunObserverSuffix(int i) => _resolve().MouXiahouDunObserverSuffix(i);
    internal int MouXiahouDunPhaseUses(int seat, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouXiahouDunPhaseUses(seat, phase);
    internal global::CardGame.Core.PublicPersistentPileSource? MouXiahouDunPile(int seat, string instance) => _resolve().MouXiahouDunPile(seat, instance);
    internal int MouXiahouDunProjectedUses(int seat, string skill, int fallback) => _resolve().MouXiahouDunProjectedUses(seat, skill, fallback);
    internal int MouXiahouDunQingjianCapacity(int seat) => _resolve().MouXiahouDunQingjianCapacity(seat);
    internal int MouXiahouDunQingjianCount(int seat) => _resolve().MouXiahouDunQingjianCount(seat);
    internal void PublishMouXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouXiahouDun(f);
    internal void RecordMouXiahouDunDiscardOrigins(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().RecordMouXiahouDunDiscardOrigins(batch);
    internal bool ResumeMouXiahouDun(long id) => _resolve().ResumeMouXiahouDun(id);
    internal bool ReturnMouXiahouDunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouXiahouDunMovement(f);
    internal void SealMouXiahouDunMovements(long id, long before) => _resolve().SealMouXiahouDunMovements(id, before);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouXiahouDunMobile(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().StartMouXiahouDunMobile(supplied, op);
    internal bool ValidMouXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouXiahouDun(f);
    internal bool HasMouXiahouDunCapability { get => _resolve().HasMouXiahouDunCapability; }
}
