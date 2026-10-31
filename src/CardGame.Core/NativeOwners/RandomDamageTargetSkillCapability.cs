#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RandomDamageTargetSkillCapability(Func<RuntimeEntries.CoreGameEngineRandomDamageTargetSkillsEntries> getCoreGameEngineRandomDamageTargetSkillsRuntime) : IRandomDamageTargetSkillHost
{
    public SkillProgramStepOutcome AcquireRandomDamageTargetSkill(ProgramSkillFrame frame) => getCoreGameEngineRandomDamageTargetSkillsRuntime().AcquireRandomDamageTargetSkill(frame);
}
