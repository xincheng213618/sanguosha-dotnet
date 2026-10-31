#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMaMiaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaMiaoAnniversaryEntries> getCharacterMaMiaoAnniversaryRuntime) : IAnniversaryMaMiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaMiaoAnniversaryRuntime().BeginMaMiao(f, op);
}
