#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuoZhaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries> getCharacterGuoZhaoAnniversaryRuntime) : IAnniversaryGuoZhaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoZhaoAnniversaryRuntime().BeginGuoZhao(f, op);
}
