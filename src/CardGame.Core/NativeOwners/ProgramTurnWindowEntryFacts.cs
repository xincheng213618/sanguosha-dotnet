#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowEntryFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramTurnWindowEntryFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public ActualTurnKind TurnKind => actualTurnProgressState.ReferenceToTurnProgression.Kind;
    public bool HasHongjiCapability => getCharacterZhangShiPingOlRuntime().HasHongjiCapability;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
