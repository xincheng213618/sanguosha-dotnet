#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SunHaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunHaoOlEntries> getCharacterSunHaoOlRuntime) : ISunHaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHaoOlRuntime().BeginSunHao(f, op);
}
