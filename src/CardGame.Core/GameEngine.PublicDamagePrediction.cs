namespace CardGame.Core;

public sealed partial class GameEngine
{

    /// <summary>Pure estimate of the viewer's proposed card and visible table; never issues an attempt or consumes usage.</summary>
    public PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, LegalAction action, int targetSeat)
=>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime>().PredictPublicActionDamage(viewerSeat, action, targetSeat);
}
