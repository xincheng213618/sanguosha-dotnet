#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGongsunZanMobileCapability(Func<RuntimeEntries.CharacterGongsunZanMouMobileEntries> getCharacterGongsunZanMouMobileRuntime) : IMouGongsunZanMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGongsunZanMouMobileRuntime().BeginMouGszMobile(f, op);
}
