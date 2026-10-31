#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ExtraEffectsAndRankGuessCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectsAndRankGuessingEntries> getCoreGameEngineExtraEffectsAndRankGuessingRuntime) : IExtraEffectsAndRankGuessHost
{
    public SkillProgramStepOutcome BeginExtraEffectsAndRankGuess(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineExtraEffectsAndRankGuessingRuntime().BeginExtraEffectsAndRankGuess(effect, frame);
}
