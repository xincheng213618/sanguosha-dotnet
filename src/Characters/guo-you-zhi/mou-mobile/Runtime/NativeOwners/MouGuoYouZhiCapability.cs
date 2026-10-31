#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoYouZhiCapability(Func<RuntimeEntries.CharacterGuoYouZhiMouMobileEntries> getCharacterGuoYouZhiMouMobileRuntime) : IMouGuoYouZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoYouZhiMouMobileRuntime().BeginMouGyz(f, op);
}
