#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouHuiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouHuiAnniversaryEntries> getCharacterXiahouHuiAnniversaryRuntime) : IAnniversaryXiahouHuiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiahouHuiAnniversaryRuntime().BeginXiahouHui(f, operation);
}
