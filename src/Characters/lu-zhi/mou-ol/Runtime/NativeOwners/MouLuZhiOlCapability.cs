#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuZhiOlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouOlEntries> getCharacterLuZhiMouOlRuntime) : IMouLuZhiOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuZhiMouOlRuntime().BeginMouLz(f, op);
}
