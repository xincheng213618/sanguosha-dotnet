#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LuZhiWeiProgramCapability(Func<RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime) : ILuZhiWeiProgramHost
{
    public SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().DrawAndArmQingzhongActualPlayDebt(f);
    public SkillProgramStepOutcome SettleQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().SettleQingzhongActualPlayDebt(f);
}
