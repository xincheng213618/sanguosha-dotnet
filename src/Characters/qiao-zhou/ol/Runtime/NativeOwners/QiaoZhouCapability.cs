#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QiaoZhouCapability(Func<RuntimeEntries.CharacterQiaoZhouOlEntries> getCharacterQiaoZhouOlRuntime) : IQiaoZhouProgramHost
{
    public SkillProgramStepOutcome Forecast(ProgramSkillFrame frame) => getCharacterQiaoZhouOlRuntime().BeginQiaoZhouForecast(frame);
}
