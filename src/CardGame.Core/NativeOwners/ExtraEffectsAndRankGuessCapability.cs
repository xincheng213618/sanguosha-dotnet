#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ExtraEffectsAndRankGuessCapability(Func<RuntimeEntries.CoreGameEngineExtraEffectsAndRankGuessingEntries> getCoreGameEngineExtraEffectsAndRankGuessingRuntime) : IExtraEffectsAndRankGuessHost
{
    public SkillProgramStepOutcome BeginExtraEffectsAndRankGuess(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineExtraEffectsAndRankGuessingRuntime().BeginExtraEffectsAndRankGuess(effect, frame);
}
