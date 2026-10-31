#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenSunCeCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunCeMobileGodEntries> getCharacterSunCeMobileGodRuntime) : IShenSunCeProgramHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunCeMobileGodRuntime().BeginShenSunCe(f, op);
}
