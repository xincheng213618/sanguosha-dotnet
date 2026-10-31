#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameActivationAwakeningProgramCapability(Func<RuntimeEntries.CoreGameEngineGameActivationAwakeningEntries> getCoreGameEngineGameActivationAwakeningRuntime) : IGameActivationAwakeningProgramHost
{
    public SkillProgramStepOutcome BeginGameActivationAwakening(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineGameActivationAwakeningRuntime().BeginGameActivationAwakening(f, e);
}
