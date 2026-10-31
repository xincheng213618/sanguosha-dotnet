#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouSunJianOlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries> getCharacterSunJianMouOlRuntime) : IMouSunJianOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouSunJianYipo ? getCharacterSunJianMouOlRuntime().BeginMouSunJianOlYipo(f) : getCharacterSunJianMouOlRuntime().BeginMouSunJianOlHulie(f, op);
}
