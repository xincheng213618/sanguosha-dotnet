#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouXuHuangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuHuangMouMobileEntries> getCharacterXuHuangMouMobileRuntime) : IMouXuHuangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouXuHuangDuanliang ? getCharacterXuHuangMouMobileRuntime().BeginMouXuHuangDuanliang(f) : getCharacterXuHuangMouMobileRuntime().BeginMouXuHuangShipo(f);
}
