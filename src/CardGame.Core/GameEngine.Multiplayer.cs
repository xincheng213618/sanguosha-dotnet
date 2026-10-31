namespace CardGame.Core;

public sealed partial class GameEngine
{

    /// <summary>Only the acting seat receives its legal actions; this query never advances the match.</summary>
    public IReadOnlyList<LegalAction> GetLegalActions(int actorSeat)
=>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime>().GetLegalActions(actorSeat);
}
