#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JiangGanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangGanOlEntries> getCharacterJiangGanOlRuntime) : IJiangGanProgramHost
{
    public SkillProgramStepOutcome DaoshuGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterJiangGanOlRuntime().DaoshuProgramGuessAndTake(frame, effect);
}
