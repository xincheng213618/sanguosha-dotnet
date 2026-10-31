#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PhaseNamePredictionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime) : IPhaseNamePredictionHost
{
    public SkillProgramStepOutcome BeginPhaseNamePrediction(ProgramSkillFrame f, string state) => getCoreGameEnginePhaseNamePredictionRuntime().BeginPhaseNamePrediction(f, state);
    public SkillProgramStepOutcome SettlePhaseNamePrediction(ProgramSkillFrame f, string state) => getCoreGameEnginePhaseNamePredictionRuntime().SettlePhaseNamePrediction(f, state);
}
