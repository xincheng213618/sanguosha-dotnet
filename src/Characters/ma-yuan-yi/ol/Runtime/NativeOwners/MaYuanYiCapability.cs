#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MaYuanYiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaYuanYiOlEntries> getCharacterMaYuanYiOlRuntime) : IMaYuanYiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaYuanYiOlRuntime().BeginMaYuanYi(f, op);
}
