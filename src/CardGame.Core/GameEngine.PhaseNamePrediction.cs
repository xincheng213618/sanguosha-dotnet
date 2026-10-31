using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.PhaseNamePredictionPolicy> _phaseNamePredictions => ref _phaseNamePrediction.ReferenceToPhaseNamePredictions;
}
