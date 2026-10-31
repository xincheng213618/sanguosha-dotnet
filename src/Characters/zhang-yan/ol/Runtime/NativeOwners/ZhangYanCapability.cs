#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangYanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries> getCharacterZhangYanOlRuntime) : IZhangYanHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYanOlRuntime().BeginZhangYan(f, op);
}
