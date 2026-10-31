#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouGongsunZanMobileCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries> getCharacterGongsunZanMouMobileRuntime) : IMouGongsunZanMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGongsunZanMouMobileRuntime().BeginMouGszMobile(f, op);
}
