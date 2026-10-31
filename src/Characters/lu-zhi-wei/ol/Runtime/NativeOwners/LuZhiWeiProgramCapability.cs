#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class LuZhiWeiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime) : ILuZhiWeiProgramHost
{
    public SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().DrawAndArmQingzhongActualPlayDebt(f);
    public SkillProgramStepOutcome SettleQingzhongActualPlayDebt(ProgramSkillFrame f) => getCharacterLuZhiWeiOlRuntime().SettleQingzhongActualPlayDebt(f);
}
