#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterNiuFuOlEntries
{
    internal void AddNiuFuStep(long id, NiuFuStage stage, string reason, long before, int count, Card? card = default, CardLocation? from = default, CardLocation? to = default) => _resolve().AddNiuFuStep(id, stage, reason, before, count, card, from, to);
    internal bool AdvanceNiuFuSubtree() => _resolve().AdvanceNiuFuSubtree();
    internal PromptChoice AiNiuFu(PendingDecision p, ProgramSkillFrame f) => _resolve().AiNiuFu(p, f);
    internal void AnswerNiuFu(PromptChoice choice) => _resolve().AnswerNiuFu(choice);
    internal SkillProgramStepOutcome BeginNiuFu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginNiuFu(supplied, op);
    internal void ContinueNiuFu(long id) => _resolve().ContinueNiuFu(id);
    internal bool ContinuePaidNiuFu(ProgramSkillFrame f) => _resolve().ContinuePaidNiuFu(f);
    internal bool DrainNiuFu(long id, bool movement) => _resolve().DrainNiuFu(id, movement);
    internal void FinishNiuFu(ProgramSkillFrame f) => _resolve().FinishNiuFu(f);
    internal bool IsNiuFuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsNiuFuRecoveryParent(f, recovery);
    internal bool IsNiuFuZonglueDamage(int owner, ProgramSkillWindowContext c) => _resolve().IsNiuFuZonglueDamage(owner, c);
    internal IReadOnlyList<PromptChoice> NiuFuChoices(ProgramSkillFrame f) => _resolve().NiuFuChoices(f);
    internal bool NiuFuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().NiuFuFirstChild(f, child);
    internal bool NiuFuLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => _resolve().NiuFuLionProof(f, seat, reason);
    internal ProgramSkillFrame? NiuFuObserverRoot() => _resolve().NiuFuObserverRoot();
    internal GameEngine.IDamageAttempt? NiuFuOriginalAttack(long damageId) => _resolve().NiuFuOriginalAttack(damageId);
    internal IReadOnlyList<PromptChoice> NiuFuSourceEquipment(ProgramSkillFrame f) => _resolve().NiuFuSourceEquipment(f);
    internal void PublishNiuFu(ProgramSkillFrame f) => _resolve().PublishNiuFu(f);
    internal bool QualifyNiuFu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().QualifyNiuFu(candidate, trigger, c);
    internal bool ResumeNiuFu(long id) => _resolve().ResumeNiuFu(id);
    internal bool ReturnNiuFuMovement(ProgramSkillFrame f) => _resolve().ReturnNiuFuMovement(f);
}
