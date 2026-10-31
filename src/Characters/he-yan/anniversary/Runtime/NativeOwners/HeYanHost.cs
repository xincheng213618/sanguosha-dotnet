#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HeYanHost(Func<RuntimeEntries.CharacterHeYanAnniversaryEntries> getCharacterHeYanAnniversaryRuntime) : IAnniversaryHeYanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHeYanAnniversaryRuntime().BeginHeYan(f, op);
}
