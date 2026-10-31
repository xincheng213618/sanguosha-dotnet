#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameActivationAwakeningProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGameActivationAwakeningEntries> getCoreGameEngineGameActivationAwakeningRuntime) : IGameActivationAwakeningProgramHost
{
    public SkillProgramStepOutcome BeginGameActivationAwakening(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineGameActivationAwakeningRuntime().BeginGameActivationAwakening(f, e);
}
