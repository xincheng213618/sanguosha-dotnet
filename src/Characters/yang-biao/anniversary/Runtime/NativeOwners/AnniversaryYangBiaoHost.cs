#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYangBiaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYangBiaoAnniversaryEntries> getCharacterYangBiaoAnniversaryRuntime) : IAnniversaryYangBiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYangBiaoAnniversaryRuntime().BeginYangBiao(f, op);
}
