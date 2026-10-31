#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYanXiangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanXiangAnniversaryEntries> getCharacterYanXiangAnniversaryRuntime) : IAnniversaryYanXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanXiangAnniversaryRuntime().BeginYanXiang(f, op);
}
