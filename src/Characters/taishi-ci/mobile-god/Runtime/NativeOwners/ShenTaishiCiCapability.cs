#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenTaishiCiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaishiCiMobileGodEntries> getCharacterTaishiCiMobileGodRuntime) : IShenTaishiCiHost
{
    public SkillProgramStepOutcome Powei(ProgramSkillFrame f) => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscPowei(f);
    public void Dulie(ProgramSkillFrame f) => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscDulie(f);
}
