#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangAnniversaryEntries
{
    internal bool AdvanceGaoXiangSubtree() => _resolve().AdvanceGaoXiangSubtree();
    internal void AnswerGaoXiang(PromptChoice answer) => _resolve().AnswerGaoXiang(answer);
    internal SkillProgramStepOutcome BeginGaoXiang(ProgramSkillFrame supplied) => _resolve().BeginGaoXiang(supplied);
    internal bool ContinueGaoXiang(long id) => _resolve().ContinueGaoXiang(id);
    internal bool DrainGaoXiang(ProgramSkillFrame f) => _resolve().DrainGaoXiang(f);
    internal PromptChoice GaoXiangAi(PendingDecision p, ProgramSkillFrame f) => _resolve().GaoXiangAi(p, f);
    internal IReadOnlyList<PromptChoice> GaoXiangCardChoices(ProgramSkillFrame f, int victim) => _resolve().GaoXiangCardChoices(f, victim);
    internal IReadOnlyList<PromptChoice> GaoXiangChoices(ProgramSkillFrame f) => _resolve().GaoXiangChoices(f);
    internal int GaoXiangChooser(ProgramSkillFrame f) => _resolve().GaoXiangChooser(f);
    internal bool GaoXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().GaoXiangFirstChild(f, child);
    internal int GaoXiangHp(ProgramSkillFrame f, int seat) => _resolve().GaoXiangHp(f, seat);
    internal bool GaoXiangMoveExists(ProgramSkillFrame f, GaoXiangChiyingMove receipt, bool gain) => _resolve().GaoXiangMoveExists(f, receipt, gain);
    internal ProgramSkillFrame? GaoXiangObserverRoot() => _resolve().GaoXiangObserverRoot();
    internal bool GaoXiangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().GaoXiangRecoveryParent(f, recovery);
    internal CardConversionSource GaoXiangSource(ProgramSkillFrame f) => _resolve().GaoXiangSource(f);
    internal IEnumerable<int> GaoXiangVictimCandidates(ProgramSkillFrame f) => _resolve().GaoXiangVictimCandidates(f);
    internal bool KeepGaoXiangCommitted(ProgramSkillFrame f) => _resolve().KeepGaoXiangCommitted(f);
    internal void PublishGaoXiang(ProgramSkillFrame f) => _resolve().PublishGaoXiang(f);
    internal void RequireGaoXiang(ProgramSkillFrame f) => _resolve().RequireGaoXiang(f);
    internal bool ReturnGaoXiangMovement(ProgramSkillFrame f) => _resolve().ReturnGaoXiangMovement(f);
    internal bool ValidGaoXiang(ProgramSkillFrame f) => _resolve().ValidGaoXiang(f);
}
