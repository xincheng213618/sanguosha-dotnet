#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeckProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckProgramsEntries> getCoreGameEngineDeckProgramsRuntime) : IDeckProgramHost
{
    public SkillProgramStepOutcome ExecuteDeckProgram(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineDeckProgramsRuntime().ExecuteDeckProgram(effect, frame);
}
