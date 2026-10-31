#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHengAnniversaryEntries
{
    internal bool AdvanceZhangHengChildren() => _resolve().AdvanceZhangHengChildren();
    internal void AnswerZhangHeng(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerZhangHeng(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangHengDangzai(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginZhangHengDangzai(supplied);
    internal global::CardGame.Core.AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().CaptureAnniversaryZhangHengMovement(card, from, to);
    internal global::CardGame.Core.PromptChoice ChooseZhangHeng(global::CardGame.Core.PendingDecision p) => _resolve().ChooseZhangHeng(p);
    internal bool ContinueZhangHeng(long id) => _resolve().ContinueZhangHeng(id);
    internal void FinishZhangHeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhangHeng(f);
    internal void PublishZhangHeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangHeng(f);
    internal bool QualifyAnniversaryZhangHeng(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().QualifyAnniversaryZhangHeng(c, t, w);
    internal bool RetainZhangHeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainZhangHeng(f);
    internal bool ReturnZhangHengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangHengMovement(f);
    internal bool ValidZhangHeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangHeng(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangHengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHengChoices(f);
    internal global::System.Collections.Generic.IEnumerable<int> ZhangHengDonors(int owner) => _resolve().ZhangHengDonors(owner);
    internal bool ZhangHengDying() => _resolve().ZhangHengDying();
    internal bool ZhangHengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangHengFirstChild(f, child);
    internal global::CardGame.Core.Card[] ZhangHengMovable(int owner, int donor, global::System.Collections.Generic.IReadOnlyList<int>? selected = default) => _resolve().ZhangHengMovable(owner, donor, selected);
    internal bool ZhangHengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ZhangHengMovement(f, e, p);
    internal bool ZhangHengObserverEdge(int i) => _resolve().ZhangHengObserverEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangHengObserverRoot() => _resolve().ZhangHengObserverRoot();
    internal bool ZhangHengOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHengOriginal(f);
    internal bool ZhangHengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ZhangHengRecoveryParent(f, recovery);
}
