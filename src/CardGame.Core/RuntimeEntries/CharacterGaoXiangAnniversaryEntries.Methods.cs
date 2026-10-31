#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangAnniversaryEntries
{
    internal bool AdvanceGaoXiangSubtree() => _resolve().AdvanceGaoXiangSubtree();
    internal void AnswerGaoXiang(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerGaoXiang(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGaoXiang(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginGaoXiang(supplied);
    internal bool ContinueGaoXiang(long id) => _resolve().ContinueGaoXiang(id);
    internal bool DrainGaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainGaoXiang(f);
    internal global::CardGame.Core.PromptChoice GaoXiangAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().GaoXiangAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GaoXiangCardChoices(global::CardGame.Core.ProgramSkillFrame f, int victim) => _resolve().GaoXiangCardChoices(f, victim);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GaoXiangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GaoXiangChoices(f);
    internal int GaoXiangChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GaoXiangChooser(f);
    internal bool GaoXiangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().GaoXiangFirstChild(f, child);
    internal int GaoXiangHp(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().GaoXiangHp(f, seat);
    internal bool GaoXiangMoveExists(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GaoXiangChiyingMove receipt, bool gain) => _resolve().GaoXiangMoveExists(f, receipt, gain);
    internal global::CardGame.Core.ProgramSkillFrame? GaoXiangObserverRoot() => _resolve().GaoXiangObserverRoot();
    internal bool GaoXiangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().GaoXiangRecoveryParent(f, recovery);
    internal global::CardGame.Core.CardConversionSource GaoXiangSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GaoXiangSource(f);
    internal global::System.Collections.Generic.IEnumerable<int> GaoXiangVictimCandidates(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GaoXiangVictimCandidates(f);
    internal bool KeepGaoXiangCommitted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepGaoXiangCommitted(f);
    internal void PublishGaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGaoXiang(f);
    internal void RequireGaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireGaoXiang(f);
    internal bool ReturnGaoXiangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGaoXiangMovement(f);
    internal bool ValidGaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGaoXiang(f);
}
