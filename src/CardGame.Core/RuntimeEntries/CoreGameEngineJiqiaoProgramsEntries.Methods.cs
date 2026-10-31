#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoProgramsEntries
{
    internal void CompletePaidJiqiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletePaidJiqiao(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecutePaidJiqiao(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecutePaidJiqiao(input, effect);
    internal void IssueJiqiaoStage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueJiqiaoStage(f);
    internal global::CardGame.Core.Card JiqiaoCard(int id) => _resolve().JiqiaoCard(id);
    internal bool ResumePaidJiqiao(long id) => _resolve().ResumePaidJiqiao(id);
    internal bool ReturnPaidJiqiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPaidJiqiaoMovement(f);
}
