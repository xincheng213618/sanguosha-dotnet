#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SunYuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries> getCharacterSunYuanOlRuntime) : ISunYuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunYuanOlRuntime().BeginSunYuan(f, op);
}
