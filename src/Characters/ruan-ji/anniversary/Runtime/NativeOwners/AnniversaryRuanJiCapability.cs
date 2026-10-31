#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryRuanJiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterRuanJiAnniversaryEntries> getCharacterRuanJiAnniversaryRuntime) : IAnniversaryRuanJiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterRuanJiAnniversaryRuntime().BeginRuanJi(f, op);
}
