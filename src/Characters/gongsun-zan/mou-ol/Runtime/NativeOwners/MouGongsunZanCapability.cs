#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGongsunZanCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGongsunZanMouOlRuntime>> getCharacterGongsunZanMouOlRuntime) : IMouGongsunZanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouGongsunZanJiaodi ? getCharacterGongsunZanMouOlRuntime().Value.BeginMouGszJiaodi(f) : getCharacterGongsunZanMouOlRuntime().Value.BeginMouGszBaojing(f);
}
