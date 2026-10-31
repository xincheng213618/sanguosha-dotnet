#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeFacts(EngineStates.CoreMatchSetupState matchSetupState, Func<RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IGameStartingDrawNativeFacts
{
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public bool TracksNativeDrawInvocations => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().TracksNativeDrawInvocations;

    public bool HasOperationReturn(SkillProgramEffectOp operation) => EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
}
