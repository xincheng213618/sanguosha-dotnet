#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JiangGanProgramCapability(Func<RuntimeEntries.CharacterJiangGanOlEntries> getCharacterJiangGanOlRuntime) : IJiangGanProgramHost
{
    public SkillProgramStepOutcome DaoshuGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterJiangGanOlRuntime().DaoshuProgramGuessAndTake(frame, effect);
}
