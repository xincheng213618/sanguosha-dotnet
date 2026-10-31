#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenTaishiCiCapability(Func<RuntimeEntries.CharacterTaishiCiMobileGodEntries> getCharacterTaishiCiMobileGodRuntime) : IShenTaishiCiHost
{
    public SkillProgramStepOutcome Powei(ProgramSkillFrame f) => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscPowei(f);
    public void Dulie(ProgramSkillFrame f) => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscDulie(f);
}
