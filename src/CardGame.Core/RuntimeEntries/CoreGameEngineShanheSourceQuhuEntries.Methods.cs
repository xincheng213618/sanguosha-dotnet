#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQuhuEntries
{
    internal bool AllowsShanheSourceQuhu(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsShanheSourceQuhu(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome DamageShanheQuhuOpponentMaximumHp(global::CardGame.Core.ProgramSkillFrame supplied, int targetSeat) => _resolve().DamageShanheQuhuOpponentMaximumHp(supplied, targetSeat);
}
