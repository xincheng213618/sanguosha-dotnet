#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryRuanJiCapability(Func<RuntimeEntries.CharacterRuanJiAnniversaryEntries> getCharacterRuanJiAnniversaryRuntime) : IAnniversaryRuanJiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterRuanJiAnniversaryRuntime().BeginRuanJi(f, op);
}
