#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhangxiuEntries
{
    internal bool CanPayShanheS5Xiongluan(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanPayShanheS5Xiongluan(owner, activation);
    internal bool ContinuePaidShanheS5Xiongluan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShanheS5Xiongluan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PayShanheS5Xiongluan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().PayShanheS5Xiongluan(input);
    internal bool ResumeShanheS5Xiongluan(long id) => _resolve().ResumeShanheS5Xiongluan(id);
    internal bool ReturnShanheS5XiongluanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShanheS5XiongluanMovement(f);
}
