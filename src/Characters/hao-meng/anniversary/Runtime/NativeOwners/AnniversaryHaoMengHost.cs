#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHaoMengHost(Func<RuntimeEntries.CharacterHaoMengAnniversaryEntries> getCharacterHaoMengAnniversaryRuntime) : IAnniversaryHaoMengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHaoMengAnniversaryRuntime().BeginHaoMeng(f, op);
}
