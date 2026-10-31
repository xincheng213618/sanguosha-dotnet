#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoHuaiCapability(Func<RuntimeEntries.CharacterGuoHuaiMouMobileEntries> getCharacterGuoHuaiMouMobileRuntime) : IMouGuoHuaiMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoHuaiMouMobileRuntime().BeginMouGh(f, op);
}
