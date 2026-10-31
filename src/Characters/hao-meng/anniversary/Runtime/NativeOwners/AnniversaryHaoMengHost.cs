#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHaoMengHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHaoMengAnniversaryEntries> getCharacterHaoMengAnniversaryRuntime) : IAnniversaryHaoMengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHaoMengAnniversaryRuntime().BeginHaoMeng(f, op);
}
