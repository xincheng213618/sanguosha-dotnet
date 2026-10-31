#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoZhongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime) : IZhaoZhongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhaoZhongOlRuntime().BeginZhaoZhong(f, op);
}
