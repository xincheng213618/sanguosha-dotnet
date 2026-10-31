#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQuhuEntries
{
    internal bool AllowsShanheSourceQuhu(ProgramActivationLegalityContext gate) => _resolve().AllowsShanheSourceQuhu(gate);
    internal SkillProgramStepOutcome DamageShanheQuhuOpponentMaximumHp(ProgramSkillFrame supplied, int targetSeat) => _resolve().DamageShanheQuhuOpponentMaximumHp(supplied, targetSeat);
}
