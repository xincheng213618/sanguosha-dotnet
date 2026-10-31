#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowEntryFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangShiPingOlRuntime>> getCharacterZhangShiPingOlRuntime, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramTurnWindowEntryFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public ActualTurnKind TurnKind => actualTurnProgressState.ReferenceToTurnProgression.Kind;
    public bool HasHongjiCapability => getCharacterZhangShiPingOlRuntime().Value.HasHongjiCapability;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
