#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQuhuCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceQuhuEntries> getCoreGameEngineShanheSourceQuhuRuntime) : IShanheSourceQuhuHost
{
    public SkillProgramStepOutcome DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineShanheSourceQuhuRuntime().DamageShanheQuhuOpponentMaximumHp(frame, targetSeat);
}
