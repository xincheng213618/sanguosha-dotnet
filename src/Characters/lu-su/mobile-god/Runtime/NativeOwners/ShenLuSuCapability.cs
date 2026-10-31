#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenLuSuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileGodEntries> getCharacterLuSuMobileGodRuntime) : IShenLuSuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuSuMobileGodRuntime().BeginShenLs(f, op);
}
