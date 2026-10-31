#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQuhuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceQuhuRuntime>> getCoreGameEngineShanheSourceQuhuRuntime) : IShanheSourceQuhuHost
{
    public SkillProgramStepOutcome DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineShanheSourceQuhuRuntime().Value.DamageShanheQuhuOpponentMaximumHp(frame, targetSeat);
}
