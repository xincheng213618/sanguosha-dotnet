#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageCardColorHealthCapability(Func<RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries> getCoreGameEngineDamageCardColorHealthRuntime) : IDamageCardColorHealthHost
{
    public SkillProgramStepOutcome ResolveDamageCardColorHealth(ProgramSkillFrame frame) => getCoreGameEngineDamageCardColorHealthRuntime().ResolveDamageCardColorHealth(frame);
}
