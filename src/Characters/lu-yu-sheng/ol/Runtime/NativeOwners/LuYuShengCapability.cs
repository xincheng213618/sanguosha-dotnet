#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LuYuShengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuYuShengOlEntries> getCharacterLuYuShengOlRuntime) : ILuYuShengHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuYuShengOlRuntime().BeginLuYuSheng(f, op);
}
