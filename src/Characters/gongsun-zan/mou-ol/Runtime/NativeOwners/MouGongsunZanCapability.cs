#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouGongsunZanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouOlEntries> getCharacterGongsunZanMouOlRuntime) : IMouGongsunZanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouGongsunZanJiaodi ? getCharacterGongsunZanMouOlRuntime().BeginMouGszJiaodi(f) : getCharacterGongsunZanMouOlRuntime().BeginMouGszBaojing(f);
}
