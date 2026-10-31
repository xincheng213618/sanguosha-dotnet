#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGongsunZanCapability(Func<RuntimeEntries.CharacterGongsunZanMouOlEntries> getCharacterGongsunZanMouOlRuntime) : IMouGongsunZanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouGongsunZanJiaodi ? getCharacterGongsunZanMouOlRuntime().BeginMouGszJiaodi(f) : getCharacterGongsunZanMouOlRuntime().BeginMouGszBaojing(f);
}
