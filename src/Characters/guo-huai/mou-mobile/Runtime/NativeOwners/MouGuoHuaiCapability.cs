#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoHuaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoHuaiMouMobileEntries> getCharacterGuoHuaiMouMobileRuntime) : IMouGuoHuaiMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoHuaiMouMobileRuntime().BeginMouGh(f, op);
}
