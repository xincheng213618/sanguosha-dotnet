#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHengAnniversaryEntries
{
    internal bool AdvanceZhangHengChildren() => _resolve().AdvanceZhangHengChildren();
    internal void AnswerZhangHeng(PromptChoice selected) => _resolve().AnswerZhangHeng(selected);
    internal SkillProgramStepOutcome BeginZhangHengDangzai(ProgramSkillFrame supplied) => _resolve().BeginZhangHengDangzai(supplied);
    internal AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to) => _resolve().CaptureAnniversaryZhangHengMovement(card, from, to);
    internal PromptChoice ChooseZhangHeng(PendingDecision p) => _resolve().ChooseZhangHeng(p);
    internal bool ContinueZhangHeng(long id) => _resolve().ContinueZhangHeng(id);
    internal void FinishZhangHeng(ProgramSkillFrame f) => _resolve().FinishZhangHeng(f);
    internal void PublishZhangHeng(ProgramSkillFrame f) => _resolve().PublishZhangHeng(f);
    internal bool QualifyAnniversaryZhangHeng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().QualifyAnniversaryZhangHeng(c, t, w);
    internal bool RetainZhangHeng(ProgramSkillFrame f) => _resolve().RetainZhangHeng(f);
    internal bool ReturnZhangHengMovement(ProgramSkillFrame f) => _resolve().ReturnZhangHengMovement(f);
    internal bool ValidZhangHeng(ProgramSkillFrame f) => _resolve().ValidZhangHeng(f);
    internal IReadOnlyList<PromptChoice> ZhangHengChoices(ProgramSkillFrame f) => _resolve().ZhangHengChoices(f);
    internal IEnumerable<int> ZhangHengDonors(int owner) => _resolve().ZhangHengDonors(owner);
    internal bool ZhangHengDying() => _resolve().ZhangHengDying();
    internal bool ZhangHengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangHengFirstChild(f, child);
    internal Card[] ZhangHengMovable(int owner, int donor, IReadOnlyList<int>? selected = default) => _resolve().ZhangHengMovable(owner, donor, selected);
    internal bool ZhangHengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().ZhangHengMovement(f, e, p);
    internal bool ZhangHengObserverEdge(int i) => _resolve().ZhangHengObserverEdge(i);
    internal ProgramSkillFrame? ZhangHengObserverRoot() => _resolve().ZhangHengObserverRoot();
    internal bool ZhangHengOriginal(ProgramSkillFrame f) => _resolve().ZhangHengOriginal(f);
    internal bool ZhangHengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ZhangHengRecoveryParent(f, recovery);
}
