#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RandomDamageTargetSkillCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomDamageTargetSkillsEntries> getCoreGameEngineRandomDamageTargetSkillsRuntime) : IRandomDamageTargetSkillHost
{
    public SkillProgramStepOutcome AcquireRandomDamageTargetSkill(ProgramSkillFrame frame) => getCoreGameEngineRandomDamageTargetSkillsRuntime().AcquireRandomDamageTargetSkill(frame);
}
