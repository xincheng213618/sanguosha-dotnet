#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhangxiuEntries
{
    internal bool CanPayShanheS5Xiongluan(CharacterState owner, SkillProgramActivation activation) => _resolve().CanPayShanheS5Xiongluan(owner, activation);
    internal bool ContinuePaidShanheS5Xiongluan(ProgramSkillFrame f) => _resolve().ContinuePaidShanheS5Xiongluan(f);
    internal SkillProgramStepOutcome PayShanheS5Xiongluan(ProgramSkillFrame input) => _resolve().PayShanheS5Xiongluan(input);
    internal bool ResumeShanheS5Xiongluan(long id) => _resolve().ResumeShanheS5Xiongluan(id);
    internal bool ReturnShanheS5XiongluanMovement(ProgramSkillFrame f) => _resolve().ReturnShanheS5XiongluanMovement(f);
}
