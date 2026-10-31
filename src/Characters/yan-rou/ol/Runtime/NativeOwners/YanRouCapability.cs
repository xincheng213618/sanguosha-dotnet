#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class YanRouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanRouOlEntries> getCharacterYanRouOlRuntime) : IYanRouHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanRouOlRuntime().BeginYanRou(f, op);
}
