#nullable enable
using System.Globalization;
using System.Text;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuLiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuLiAnniversaryEntries> getCharacterLiuLiAnniversaryRuntime) : IAnniversaryLiuLiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuLiAnniversaryRuntime().BeginLiuLi(f, op);
}
