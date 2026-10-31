#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoAnniversaryGodEntries
{
    internal void AcquireShenHtRandomSkill(ProgramSkillFrame f) => _resolve().AcquireShenHtRandomSkill(f);
    internal void AnswerShenHt(PromptChoice choice) => _resolve().AnswerShenHt(choice);
    internal void ApplySkillInvocationLuxin(SkillInvocationFrame f, SkillInvocationReaction r) => _resolve().ApplySkillInvocationLuxin(f, r);
    internal void ArmShenHtLuxin(ProgramSkillFrame f, IReadOnlyList<int> ids) => _resolve().ArmShenHtLuxin(f, ids);
    internal void AssertSkillInvocation(SkillInvocationFrame f) => _resolve().AssertSkillInvocation(f);
    internal SkillProgramStepOutcome BeginShenHuaTuo(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenHuaTuo(supplied, op);
    internal void CaptureMovementBatchStartInvocationOrigin(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartInvocationOrigin(batch);
    internal PromptChoice ChooseShenHtHuandao(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShenHtHuandao(p, f);
    internal PromptChoice ChooseShenHtLuxin(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShenHtLuxin(p, f);
    internal void CompleteSkillInvocationDraw(SkillInvocationFrame f) => _resolve().CompleteSkillInvocationDraw(f);
    internal bool ContinueIssuedShenHt(ProgramSkillFrame f) => _resolve().ContinueIssuedShenHt(f);
    internal void ContinueShenHuaTuo(long id) => _resolve().ContinueShenHuaTuo(id);
    internal void ContinueSkillInvocation(SkillInvocationFrame supplied) => _resolve().ContinueSkillInvocation(supplied);
    internal void FinishShenHt(ProgramSkillFrame f) => _resolve().FinishShenHt(f);
    internal bool HasConvertedSkillInvocations(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().HasConvertedSkillInvocations(action, window);
    internal bool HasExactSkillInvocationDying() => _resolve().HasExactSkillInvocationDying();
    internal bool HasSkillInvocationObserverSuffix(int rootIndex) => _resolve().HasSkillInvocationObserverSuffix(rootIndex);
    internal bool IsAcceptedViewAsInvocation(CardConversionSource source) => _resolve().IsAcceptedViewAsInvocation(source);
    internal bool IsExactSkillInvocationDying(DyingFrame? dying) => _resolve().IsExactSkillInvocationDying(dying);
    internal bool IsSkillInvocationHealthChild(SkillInvocationFrame f, HpChangedTriggerWindowFrame hp) => _resolve().IsSkillInvocationHealthChild(f, hp);
    internal bool IsSkillInvocationHealthChild2(SkillInvocationFrame f, RecoveryReplacementFrame recovery) => _resolve().IsSkillInvocationHealthChild(f, recovery);
    internal bool IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch) => _resolve().IsSkillInvocationMovementBatch(f, batch);
    internal bool IsSkillInvocationRecoveryProducer(SkillInvocationFrame f, RecoveryAttempt a) => _resolve().IsSkillInvocationRecoveryProducer(f, a);
    internal void PublishShenHt(ProgramSkillFrame f) => _resolve().PublishShenHt(f);
    internal bool ResumeShenHt(long id) => _resolve().ResumeShenHt(id);
    internal void ReturnShenHtDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShenHtDraw(supplied, outcome);
    internal bool ReturnShenHtMovement(ProgramSkillFrame f) => _resolve().ReturnShenHtMovement(f);
    internal void ReturnSkillInvocationDying(GameEngine.DyingCompletionReceipt dying) => _resolve().ReturnSkillInvocationDying(dying);
    internal void RevokeShenHtSkill(ProgramSkillFrame f, string skill) => _resolve().RevokeShenHtSkill(f, skill);
    internal Card ShenHtCard(int id) => _resolve().ShenHtCard(id);
    internal IReadOnlyList<PromptChoice> ShenHtChoices(ProgramSkillFrame f) => _resolve().ShenHtChoices(f);
    internal string[] ShenHtGeneralCandidates(int seat) => _resolve().ShenHtGeneralCandidates(seat);
    internal string[] ShenHtLossSkillIds(ShenHuaTuoReceipt r) => _resolve().ShenHtLossSkillIds(r);
    internal IEnumerable<SkillGrant> ShenHtRevocableGrants(int seat, string id) => _resolve().ShenHtRevocableGrants(seat, id);
    internal IReadOnlyList<(string Skill, string Occurrence, int Actor)> SkillInvocationEquipmentSources(CardActionContext action) => _resolve().SkillInvocationEquipmentSources(action);
    internal CardConversionSource? SkillInvocationMovementOrigin() => _resolve().SkillInvocationMovementOrigin();
    internal bool SkillInvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().SkillInvocationStructuralEdge(parent, child);
    internal bool TryBeginConvertedSkillInvocations(ProgramCardTriggerWindowFrame f) => _resolve().TryBeginConvertedSkillInvocations(f);
    internal bool TryBeginProgramSkillInvocation(long id) => _resolve().TryBeginProgramSkillInvocation(id);
    internal bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, SkillInvocationReturnKind returnKind = (SkillInvocationReturnKind)0) => _resolve().TryBeginSkillInvocation(parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId, returnKind);
    internal bool HasSkillInvocationObservers { get => _resolve().HasSkillInvocationObservers; }
}
