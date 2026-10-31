#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JiangGanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterJiangGanOlRuntime>> getCharacterJiangGanOlRuntime) : IJiangGanProgramHost
{
    public SkillProgramStepOutcome DaoshuGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterJiangGanOlRuntime().Value.DaoshuProgramGuessAndTake(frame, effect);
}
