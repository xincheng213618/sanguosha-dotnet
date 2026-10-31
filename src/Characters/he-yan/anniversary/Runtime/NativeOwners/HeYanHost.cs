#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HeYanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeYanAnniversaryEntries> getCharacterHeYanAnniversaryRuntime) : IAnniversaryHeYanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHeYanAnniversaryRuntime().BeginHeYan(f, op);
}
