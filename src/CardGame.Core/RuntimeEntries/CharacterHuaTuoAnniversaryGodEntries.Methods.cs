#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoAnniversaryGodEntries
{
    internal void AcquireShenHtRandomSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AcquireShenHtRandomSkill(f);
    internal void AnswerShenHt(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShenHt(choice);
    internal void ApplySkillInvocationLuxin(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.SkillInvocationReaction r) => _resolve().ApplySkillInvocationLuxin(f, r);
    internal void ArmShenHtLuxin(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().ArmShenHtLuxin(f, ids);
    internal void AssertShenHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShenHt(f);
    internal void AssertSkillInvocation(global::CardGame.Core.SkillInvocationFrame f) => _resolve().AssertSkillInvocation(f);
    internal void AssertSkillInvocations() => _resolve().AssertSkillInvocations();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenHuaTuo(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenHuaTuo(supplied, op);
    internal void CaptureMovementBatchStartInvocationOrigin(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartInvocationOrigin(batch);
    internal global::CardGame.Core.PromptChoice ChooseShenHtHuandao(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShenHtHuandao(p, f);
    internal global::CardGame.Core.PromptChoice ChooseShenHtLuxin(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShenHtLuxin(p, f);
    internal void CompleteSkillInvocationDraw(global::CardGame.Core.SkillInvocationFrame f) => _resolve().CompleteSkillInvocationDraw(f);
    internal bool ContinueIssuedShenHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedShenHt(f);
    internal void ContinueShenHuaTuo(long id) => _resolve().ContinueShenHuaTuo(id);
    internal void ContinueSkillInvocation(global::CardGame.Core.SkillInvocationFrame supplied) => _resolve().ContinueSkillInvocation(supplied);
    internal void FinishShenHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenHt(f);
    internal bool HasConvertedSkillInvocations(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().HasConvertedSkillInvocations(action, window);
    internal bool HasExactBoundarySkillInvocation() => _resolve().HasExactBoundarySkillInvocation();
    internal bool HasExactSkillInvocationDying() => _resolve().HasExactSkillInvocationDying();
    internal bool HasSkillInvocationObserverSuffix(int rootIndex) => _resolve().HasSkillInvocationObserverSuffix(rootIndex);
    internal bool IsAcceptedViewAsInvocation(global::CardGame.Core.CardConversionSource source) => _resolve().IsAcceptedViewAsInvocation(source);
    internal bool IsCardWindowWaitingForSkillInvocation(global::CardGame.Core.ProgramCardTriggerWindowFrame window, int index) => _resolve().IsCardWindowWaitingForSkillInvocation(window, index);
    internal bool IsExactSkillInvocationDying(global::CardGame.Core.DyingFrame? dying) => _resolve().IsExactSkillInvocationDying(dying);
    internal bool IsProgramWaitingForSkillInvocation(global::CardGame.Core.ProgramSkillFrame program) => _resolve().IsProgramWaitingForSkillInvocation(program);
    internal bool IsSkillInvocationHealthChild(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => _resolve().IsSkillInvocationHealthChild(f, hp);
    internal bool IsSkillInvocationHealthChild2(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsSkillInvocationHealthChild(f, recovery);
    internal bool IsSkillInvocationMovementBatch(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsSkillInvocationMovementBatch(f, batch);
    internal bool IsSkillInvocationRecoveryProducer(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.RecoveryAttempt a) => _resolve().IsSkillInvocationRecoveryProducer(f, a);
    internal void PublishShenHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenHt(f);
    internal bool ResumeShenHt(long id) => _resolve().ResumeShenHt(id);
    internal void ReturnShenHtDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShenHtDraw(supplied, outcome);
    internal bool ReturnShenHtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenHtMovement(f);
    internal void ReturnSkillInvocationDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying) => _resolve().ReturnSkillInvocationDying(dying);
    internal void RevokeShenHtSkill(global::CardGame.Core.ProgramSkillFrame f, string skill) => _resolve().RevokeShenHtSkill(f, skill);
    internal global::CardGame.Core.Card ShenHtCard(int id) => _resolve().ShenHtCard(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenHtChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenHtChoices(f);
    internal string[] ShenHtGeneralCandidates(int seat) => _resolve().ShenHtGeneralCandidates(seat);
    internal string[] ShenHtLossSkillIds(global::CardGame.Core.ShenHuaTuoReceipt r) => _resolve().ShenHtLossSkillIds(r);
    internal bool ShenHtMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShenHtMovementProof(f, e, p);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> ShenHtRevocableGrants(int seat, string id) => _resolve().ShenHtRevocableGrants(seat, id);
    internal global::System.Collections.Generic.IReadOnlyList<(string Skill, string Occurrence, int Actor)> SkillInvocationEquipmentSources(global::CardGame.Core.CardActionContext action) => _resolve().SkillInvocationEquipmentSources(action);
    internal global::CardGame.Core.CardConversionSource? SkillInvocationMovementOrigin() => _resolve().SkillInvocationMovementOrigin();
    internal bool SkillInvocationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().SkillInvocationStructuralEdge(parent, child);
    internal bool TryBeginConvertedSkillInvocations(global::CardGame.Core.ProgramCardTriggerWindowFrame f) => _resolve().TryBeginConvertedSkillInvocations(f);
    internal bool TryBeginProgramSkillInvocation(long id) => _resolve().TryBeginProgramSkillInvocation(id);
    internal bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, global::CardGame.Core.SkillInvocationReturnKind returnKind = (global::CardGame.Core.SkillInvocationReturnKind)0) => _resolve().TryBeginSkillInvocation(parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId, returnKind);
    internal bool HasSkillInvocationObservers { get => _resolve().HasSkillInvocationObservers; }
}
