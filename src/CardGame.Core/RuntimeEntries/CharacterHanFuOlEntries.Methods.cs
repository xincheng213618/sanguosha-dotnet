#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanFuOlEntries
{
    internal bool AdvanceHanFuSubtree() => _resolve().AdvanceHanFuSubtree();
    internal PromptChoice AiHanFu(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHanFu(p, f);
    internal void AnswerHanFu(PromptChoice choice) => _resolve().AnswerHanFu(choice);
    internal SkillProgramStepOutcome BeginHanFu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHanFu(supplied, op);
    internal void ContinueHanFu(long id) => _resolve().ContinueHanFu(id);
    internal bool ContinuePaidHanFu(ProgramSkillFrame f) => _resolve().ContinuePaidHanFu(f);
    internal bool DrainHanFuChildren(long id) => _resolve().DrainHanFuChildren(id);
    internal void FinishHanFu(ProgramSkillFrame f) => _resolve().FinishHanFu(f);
    internal void FreezeKuangshouDiscard(ProgramSkillFrame f) => _resolve().FreezeKuangshouDiscard(f);
    internal IReadOnlyList<PromptChoice> HanFuChoices(ProgramSkillFrame f) => _resolve().HanFuChoices(f);
    internal IReadOnlyList<HanFuMaterial> HanFuDiscardCards(ProgramSkillFrame f) => _resolve().HanFuDiscardCards(f);
    internal bool HanFuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HanFuFirstChild(f, child);
    internal bool HanFuLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => _resolve().HanFuLionProof(f, seat, reason);
    internal HanFuMaterial HanFuMaterialFor(int id) => _resolve().HanFuMaterialFor(id);
    internal ProgramSkillFrame? HanFuObserverRoot() => _resolve().HanFuObserverRoot();
    internal bool IsHanFuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsHanFuRecoveryParent(f, recovery);
    internal int[] KuangshouDamagedSeats(int cutoff) => _resolve().KuangshouDamagedSeats(cutoff);
    internal void PayHanFuCards(ProgramSkillFrame f, IReadOnlyList<HanFuMaterial> cards, CardLocation destination, string reason, HanFuStage stage) => _resolve().PayHanFuCards(f, cards, destination, reason, stage);
    internal void PayShuziBoard(ProgramSkillFrame f, HanFuMaterial m) => _resolve().PayShuziBoard(f, m);
    internal void PublishHanFuChoice(ProgramSkillFrame f) => _resolve().PublishHanFuChoice(f);
    internal void RecordHanFuMovement(long id, HanFuStage stage, string reason, long before, int actual) => _resolve().RecordHanFuMovement(id, stage, reason, before, actual);
    internal bool ResumeHanFu(long id) => _resolve().ResumeHanFu(id);
    internal bool ReturnHanFuMovement(ProgramSkillFrame f) => _resolve().ReturnHanFuMovement(f);
    internal IReadOnlyList<HanFuMaterial> ShuziBoardCards(int target) => _resolve().ShuziBoardCards(target);
}
