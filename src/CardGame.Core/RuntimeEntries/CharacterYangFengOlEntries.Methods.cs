#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFengOlEntries
{
    internal bool AdvanceYangFengSubtree() => _resolve().AdvanceYangFengSubtree();
    internal void AnswerYangFeng(PromptChoice choice) => _resolve().AnswerYangFeng(choice);
    internal SkillProgramStepOutcome BeginYangFeng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYangFeng(supplied, op);
    internal PromptChoice ChooseYangFengAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseYangFengAi(p, f);
    internal bool ContinuePaidYangFeng(ProgramSkillFrame f) => _resolve().ContinuePaidYangFeng(f);
    internal void ContinueYangFeng(long id) => _resolve().ContinueYangFeng(id);
    internal bool DrainYangFengChildren(long id) => _resolve().DrainYangFengChildren(id);
    internal void FinishYangFeng(ProgramSkillFrame f) => _resolve().FinishYangFeng(f);
    internal bool IsJiaweiOriginalProcessing(JiaweiRevealedEntity e) => _resolve().IsJiaweiOriginalProcessing(e);
    internal bool IsYangFengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsYangFengRecoveryParent(f, recovery);
    internal IReadOnlyList<YangFengMaterial> JiaweiDiscardCandidates(ProgramSkillFrame f, int target) => _resolve().JiaweiDiscardCandidates(f, target);
    internal void MoveYangFengEntities(ProgramSkillFrame f, IReadOnlyList<YangFengMaterial> materials, CardLocation to, string reason, YangFengStage stage, long before) => _resolve().MoveYangFengEntities(f, materials, to, reason, stage, before);
    internal void PayJiaweiDiscard(ProgramSkillFrame f) => _resolve().PayJiaweiDiscard(f);
    internal void PublishYangFeng(ProgramSkillFrame f) => _resolve().PublishYangFeng(f);
    internal bool QualifyYangFeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyYangFeng(candidate, trigger, context);
    internal int QujiaPermanentSlashBonus(int ownerSeat) => _resolve().QujiaPermanentSlashBonus(ownerSeat);
    internal void RecordYangFengMovement(long id, YangFengStage stage, string reason, long before, int count) => _resolve().RecordYangFengMovement(id, stage, reason, before, count);
    internal bool ResumeYangFeng(long id) => _resolve().ResumeYangFeng(id);
    internal bool ReturnYangFengMovement(ProgramSkillFrame f) => _resolve().ReturnYangFengMovement(f);
    internal void RevealJiawei(ProgramSkillFrame f) => _resolve().RevealJiawei(f);
    internal void SettleJiawei(ProgramSkillFrame f, YangFengStage stage) => _resolve().SettleJiawei(f, stage);
    internal IReadOnlyList<PromptChoice> YangFengChoices(ProgramSkillFrame f) => _resolve().YangFengChoices(f);
    internal int YangFengCompletedTurns(int owner, int cutoff) => _resolve().YangFengCompletedTurns(owner, cutoff);
    internal bool YangFengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YangFengFirstChild(f, child);
    internal bool YangFengLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => _resolve().YangFengLionProof(f, seat, reason);
    internal ProgramSkillFrame? YangFengObserverRoot() => _resolve().YangFengObserverRoot();
}
