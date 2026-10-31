#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiBoundaryMobileEntries
{
    internal bool AdvanceBoundaryFangzhuObservers() => _resolve().AdvanceBoundaryFangzhuObservers();
    internal void AnswerBoundaryCaoPi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryCaoPi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryFangzhu(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginBoundaryFangzhu(input, e);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryXingshang(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginBoundaryXingshang(input, e);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryCaoPiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryCaoPiChoices(f);
    internal global::CardGame.Core.DeathFrame? BoundaryCaoPiDeath(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BoundaryCaoPiDeath(context);
    internal bool BoundaryFangzhuDyingObserver() => _resolve().BoundaryFangzhuDyingObserver();
    internal bool BoundaryFangzhuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryFangzhuFirstChild(f, child);
    internal global::CardGame.Core.Card[] BoundaryFangzhuMaterials(int target) => _resolve().BoundaryFangzhuMaterials(target);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryFangzhuObserverRoot() => _resolve().BoundaryFangzhuObserverRoot();
    internal bool BoundaryFangzhuRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().BoundaryFangzhuRecovery(f, recovery);
    internal void CommitBoundaryCaoPiResult(global::CardGame.Core.ProgramSkillFrame input, string bind, string outcome, int chooser, int subject, int count) => _resolve().CommitBoundaryCaoPiResult(input, bind, outcome, chooser, subject, count);
    internal void ContinueBoundaryFangzhu(long id) => _resolve().ContinueBoundaryFangzhu(id);
    internal bool KeepBoundaryFangzhuPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryFangzhuPaid(f);
    internal void PublishBoundaryCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryCaoPi(f);
    internal bool QualifyBoundaryCaoPi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoPi(c, context);
    internal bool ResumeBoundaryFangzhu(long id) => _resolve().ResumeBoundaryFangzhu(id);
    internal void ReturnBoundaryFangzhuDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryFangzhuDraw(f, outcome);
    internal bool ReturnBoundaryFangzhuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryFangzhuMovement(f);
}
