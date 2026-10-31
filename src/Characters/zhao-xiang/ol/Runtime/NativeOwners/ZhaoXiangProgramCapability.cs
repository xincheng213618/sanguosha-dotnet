#nullable enable
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoXiangProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoXiangOlEntries> getCharacterZhaoXiangOlRuntime) : IZhaoXiangProgramHost
{
    public void GainMeiYingAfterActualSlashDamage(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().GainMeiYingAfterActualSlashDamage(f);
    public SkillProgramStepOutcome ExchangeFiveShuBySpentMeiYing(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().ExchangeFiveShuBySpentMeiYing(f);
    public SkillProgramStepOutcome DrawAfterPaidMeiYingConversion(ProgramSkillFrame f) => getCharacterZhaoXiangOlRuntime().DrawAfterPaidMeiYingConversion(f);
}
