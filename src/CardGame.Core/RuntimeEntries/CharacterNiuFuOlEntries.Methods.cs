#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterNiuFuOlEntries
{
    internal void AddNiuFuStep(long id, global::CardGame.Core.NiuFuStage stage, string reason, long before, int count, global::CardGame.Core.Card? card = default, global::CardGame.Core.CardLocation? from = default, global::CardGame.Core.CardLocation? to = default) => _resolve().AddNiuFuStep(id, stage, reason, before, count, card, from, to);
    internal bool AdvanceNiuFuSubtree() => _resolve().AdvanceNiuFuSubtree();
    internal global::CardGame.Core.PromptChoice AiNiuFu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiNiuFu(p, f);
    internal void AnswerNiuFu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerNiuFu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNiuFu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginNiuFu(supplied, op);
    internal void ContinueNiuFu(long id) => _resolve().ContinueNiuFu(id);
    internal bool ContinuePaidNiuFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidNiuFu(f);
    internal bool DrainNiuFu(long id, bool movement) => _resolve().DrainNiuFu(id, movement);
    internal void FinishNiuFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishNiuFu(f);
    internal bool IsNiuFuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsNiuFuRecoveryParent(f, recovery);
    internal bool IsNiuFuZonglueDamage(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsNiuFuZonglueDamage(owner, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NiuFuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NiuFuChoices(f);
    internal bool NiuFuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().NiuFuFirstChild(f, child);
    internal bool NiuFuLionProof(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.CardMoveReason? reason) => _resolve().NiuFuLionProof(f, seat, reason);
    internal global::CardGame.Core.ProgramSkillFrame? NiuFuObserverRoot() => _resolve().NiuFuObserverRoot();
    internal global::CardGame.Core.GameEngine.IDamageAttempt? NiuFuOriginalAttack(long damageId) => _resolve().NiuFuOriginalAttack(damageId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NiuFuSourceEquipment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NiuFuSourceEquipment(f);
    internal void PublishNiuFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishNiuFu(f);
    internal bool QualifyNiuFu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().QualifyNiuFu(candidate, trigger, c);
    internal bool ResumeNiuFu(long id) => _resolve().ResumeNiuFu(id);
    internal bool ReturnNiuFuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnNiuFuMovement(f);
}
