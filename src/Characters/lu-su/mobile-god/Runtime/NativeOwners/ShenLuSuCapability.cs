#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenLuSuCapability(Func<RuntimeEntries.CharacterLuSuMobileGodEntries> getCharacterLuSuMobileGodRuntime) : IShenLuSuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuSuMobileGodRuntime().BeginShenLs(f, op);
}
