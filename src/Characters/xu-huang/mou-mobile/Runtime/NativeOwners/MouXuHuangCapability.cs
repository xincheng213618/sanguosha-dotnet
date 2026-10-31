#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouXuHuangCapability(Func<RuntimeEntries.CharacterXuHuangMouMobileEntries> getCharacterXuHuangMouMobileRuntime) : IMouXuHuangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouXuHuangDuanliang ? getCharacterXuHuangMouMobileRuntime().BeginMouXuHuangDuanliang(f) : getCharacterXuHuangMouMobileRuntime().BeginMouXuHuangShipo(f);
}
