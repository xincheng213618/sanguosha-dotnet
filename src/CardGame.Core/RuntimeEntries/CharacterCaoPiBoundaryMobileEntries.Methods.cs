#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiBoundaryMobileEntries
{
    internal bool AdvanceBoundaryFangzhuObservers() => _resolve().AdvanceBoundaryFangzhuObservers();
    internal void AnswerBoundaryCaoPi(PromptChoice choice) => _resolve().AnswerBoundaryCaoPi(choice);
    internal SkillProgramStepOutcome BeginBoundaryFangzhu(ProgramSkillFrame input, SkillProgramEffect e) => _resolve().BeginBoundaryFangzhu(input, e);
    internal SkillProgramStepOutcome BeginBoundaryXingshang(ProgramSkillFrame input, SkillProgramEffect e) => _resolve().BeginBoundaryXingshang(input, e);
    internal IReadOnlyList<PromptChoice> BoundaryCaoPiChoices(ProgramSkillFrame f) => _resolve().BoundaryCaoPiChoices(f);
    internal DeathFrame? BoundaryCaoPiDeath(ProgramSkillWindowContext context) => _resolve().BoundaryCaoPiDeath(context);
    internal bool BoundaryFangzhuDyingObserver() => _resolve().BoundaryFangzhuDyingObserver();
    internal bool BoundaryFangzhuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryFangzhuFirstChild(f, child);
    internal Card[] BoundaryFangzhuMaterials(int target) => _resolve().BoundaryFangzhuMaterials(target);
    internal ProgramSkillFrame? BoundaryFangzhuObserverRoot() => _resolve().BoundaryFangzhuObserverRoot();
    internal bool BoundaryFangzhuRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().BoundaryFangzhuRecovery(f, recovery);
    internal void CommitBoundaryCaoPiResult(ProgramSkillFrame input, string bind, string outcome, int chooser, int subject, int count) => _resolve().CommitBoundaryCaoPiResult(input, bind, outcome, chooser, subject, count);
    internal void ContinueBoundaryFangzhu(long id) => _resolve().ContinueBoundaryFangzhu(id);
    internal bool KeepBoundaryFangzhuPaid(ProgramSkillFrame f) => _resolve().KeepBoundaryFangzhuPaid(f);
    internal void PublishBoundaryCaoPi(ProgramSkillFrame f) => _resolve().PublishBoundaryCaoPi(f);
    internal bool QualifyBoundaryCaoPi(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoPi(c, context);
    internal bool ResumeBoundaryFangzhu(long id) => _resolve().ResumeBoundaryFangzhu(id);
    internal void ReturnBoundaryFangzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryFangzhuDraw(f, outcome);
    internal bool ReturnBoundaryFangzhuMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryFangzhuMovement(f);
}
