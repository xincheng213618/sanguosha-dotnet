#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouDiaoChanMobileCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanMouMobileEntries> getCharacterDiaoChanMouMobileRuntime) : IMouDiaoChanMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanMouMobileRuntime().StartMouDiaoChanMobile(f, op);
}
