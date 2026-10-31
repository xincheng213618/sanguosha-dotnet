#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuZhiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouMobileEntries> getCharacterLuZhiMouMobileRuntime) : IMouLuZhiHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuZhiMouMobileRuntime().StartMouLuZhi(f, op);
}
