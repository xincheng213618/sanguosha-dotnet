#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenSunCeCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunCeMobileGodRuntime>> getCharacterSunCeMobileGodRuntime) : IShenSunCeProgramHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunCeMobileGodRuntime().Value.BeginShenSunCe(f, op);
}
