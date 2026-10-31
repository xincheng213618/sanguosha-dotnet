#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeFacts(global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IGameStartingDrawNativeFacts
{
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public bool TracksNativeDrawInvocations => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().TracksNativeDrawInvocations;

    public bool HasOperationReturn(SkillProgramEffectOp operation) => global::CardGame.Core.EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
}
