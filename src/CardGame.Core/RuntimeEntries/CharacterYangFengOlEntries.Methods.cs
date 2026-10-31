#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFengOlEntries
{
    internal bool AdvanceYangFengSubtree() => _resolve().AdvanceYangFengSubtree();
    internal void AnswerYangFeng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYangFeng(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYangFeng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYangFeng(supplied, op);
    internal global::CardGame.Core.PromptChoice ChooseYangFengAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseYangFengAi(p, f);
    internal bool ContinuePaidYangFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidYangFeng(f);
    internal void ContinueYangFeng(long id) => _resolve().ContinueYangFeng(id);
    internal bool DrainYangFengChildren(long id) => _resolve().DrainYangFengChildren(id);
    internal void FinishYangFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYangFeng(f);
    internal bool IsJiaweiOriginalProcessing(global::CardGame.Core.JiaweiRevealedEntity e) => _resolve().IsJiaweiOriginalProcessing(e);
    internal bool IsYangFengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsYangFengRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.YangFengMaterial> JiaweiDiscardCandidates(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().JiaweiDiscardCandidates(f, target);
    internal void MoveYangFengEntities(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.YangFengMaterial> materials, global::CardGame.Core.CardLocation to, string reason, global::CardGame.Core.YangFengStage stage, long before) => _resolve().MoveYangFengEntities(f, materials, to, reason, stage, before);
    internal void PayJiaweiDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayJiaweiDiscard(f);
    internal void PublishYangFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYangFeng(f);
    internal bool QualifyYangFeng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyYangFeng(candidate, trigger, context);
    internal int QujiaPermanentSlashBonus(int ownerSeat) => _resolve().QujiaPermanentSlashBonus(ownerSeat);
    internal void RecordYangFengMovement(long id, global::CardGame.Core.YangFengStage stage, string reason, long before, int count) => _resolve().RecordYangFengMovement(id, stage, reason, before, count);
    internal bool ResumeYangFeng(long id) => _resolve().ResumeYangFeng(id);
    internal bool ReturnYangFengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYangFengMovement(f);
    internal void RevealJiawei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RevealJiawei(f);
    internal void SettleJiawei(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.YangFengStage stage) => _resolve().SettleJiawei(f, stage);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YangFengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangFengChoices(f);
    internal int YangFengCompletedTurns(int owner, int cutoff) => _resolve().YangFengCompletedTurns(owner, cutoff);
    internal bool YangFengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YangFengFirstChild(f, child);
    internal bool YangFengLionProof(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.CardMoveReason? reason) => _resolve().YangFengLionProof(f, seat, reason);
    internal global::CardGame.Core.ProgramSkillFrame? YangFengObserverRoot() => _resolve().YangFengObserverRoot();
}
