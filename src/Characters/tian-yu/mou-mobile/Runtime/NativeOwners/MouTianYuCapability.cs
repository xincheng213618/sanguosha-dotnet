#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouTianYuCapability(Func<RuntimeEntries.CharacterTianYuMouMobileEntries> getCharacterTianYuMouMobileRuntime) : IMouTianYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTianYuMouMobileRuntime().BeginMouTianYu(f, op);
}
