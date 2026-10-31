#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuZhiCapability(Func<RuntimeEntries.CharacterLuZhiMouMobileEntries> getCharacterLuZhiMouMobileRuntime) : IMouLuZhiHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuZhiMouMobileRuntime().StartMouLuZhi(f, op);
}
