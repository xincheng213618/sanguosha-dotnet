#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouTianYuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTianYuMouMobileEntries> getCharacterTianYuMouMobileRuntime) : IMouTianYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTianYuMouMobileRuntime().BeginMouTianYu(f, op);
}
