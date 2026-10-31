#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LuZhiWeiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuZhiWeiOlRuntime>> getCharacterLuZhiWeiOlRuntime) : ILuZhiWeiProgramHost
{
    public SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().Value.DrawAndArmQingzhongActualPlayDebt(f);
    public SkillProgramStepOutcome SettleQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().Value.SettleQingzhongActualPlayDebt(f);
}
