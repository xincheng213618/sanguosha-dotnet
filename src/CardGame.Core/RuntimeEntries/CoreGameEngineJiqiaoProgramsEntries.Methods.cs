#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoProgramsEntries
{
    internal void CompletePaidJiqiao(ProgramSkillFrame f) => _resolve().CompletePaidJiqiao(f);
    internal SkillProgramStepOutcome ExecutePaidJiqiao(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().ExecutePaidJiqiao(input, effect);
    internal void IssueJiqiaoStage(ProgramSkillFrame f) => _resolve().IssueJiqiaoStage(f);
    internal Card JiqiaoCard(int id) => _resolve().JiqiaoCard(id);
    internal bool ResumePaidJiqiao(long id) => _resolve().ResumePaidJiqiao(id);
    internal bool ReturnPaidJiqiaoMovement(ProgramSkillFrame f) => _resolve().ReturnPaidJiqiaoMovement(f);
}
