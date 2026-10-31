#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeckProgramCapability(Func<RuntimeEntries.CoreGameEngineDeckProgramsEntries> getCoreGameEngineDeckProgramsRuntime) : IDeckProgramHost
{
    public SkillProgramStepOutcome ExecuteDeckProgram(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineDeckProgramsRuntime().ExecuteDeckProgram(effect, frame);
}
