#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversarySunQuanCapability(Func<RuntimeEntries.CharacterSunQuanAnniversaryGodEntries> getCharacterSunQuanAnniversaryGodRuntime) : IAnniversarySunQuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterSunQuanAnniversaryGodRuntime().BeginAnniversarySunQuan(f, operation);
}
