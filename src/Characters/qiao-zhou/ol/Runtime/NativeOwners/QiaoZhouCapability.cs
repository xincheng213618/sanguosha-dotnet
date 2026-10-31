#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QiaoZhouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQiaoZhouOlEntries> getCharacterQiaoZhouOlRuntime) : IQiaoZhouProgramHost
{
    public SkillProgramStepOutcome Forecast(ProgramSkillFrame frame) => getCharacterQiaoZhouOlRuntime().BeginQiaoZhouForecast(frame);
}
