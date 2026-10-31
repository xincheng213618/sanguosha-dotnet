#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ChenDengProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime) : IChenDengProgramHost
{
    public SkillProgramStepOutcome FengjiRoundChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterChenDengOlRuntime().FengjiProgramRoundChoice(frame, effect);
    public SkillProgramStepOutcome XuanhuiSwapEffects(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterChenDengOlRuntime().XuanhuiProgramSwapEffects(frame, effect);
}
