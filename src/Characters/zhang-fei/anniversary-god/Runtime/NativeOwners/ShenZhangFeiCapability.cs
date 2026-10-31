#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhangFeiCapability(Func<RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime) : IShenZhangFeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangFeiAnniversaryGodRuntime().BeginShenZf(f, op);
}
