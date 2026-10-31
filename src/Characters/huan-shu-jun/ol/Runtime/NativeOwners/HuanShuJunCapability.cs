#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HuanShuJunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries> getCharacterHuanShuJunOlRuntime) : IHuanShuJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuanShuJunOlRuntime().BeginHuanShuJun(f, op);
}
