#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageCardColorHealthCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries> getCoreGameEngineDamageCardColorHealthRuntime) : IDamageCardColorHealthHost
{
    public SkillProgramStepOutcome ResolveDamageCardColorHealth(ProgramSkillFrame frame) => getCoreGameEngineDamageCardColorHealthRuntime().ResolveDamageCardColorHealth(frame);
}
