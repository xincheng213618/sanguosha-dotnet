#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQuhuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceQuhuEntries> getCoreGameEngineShanheSourceQuhuRuntime) : IShanheSourceQuhuHost
{
    public SkillProgramStepOutcome DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineShanheSourceQuhuRuntime().DamageShanheQuhuOpponentMaximumHp(frame, targetSeat);
}
