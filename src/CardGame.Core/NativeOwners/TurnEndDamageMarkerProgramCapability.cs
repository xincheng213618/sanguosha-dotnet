#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnEndDamageMarkerProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime>> getCoreGameEngineTurnEndDamageMarkerRuntime) : ITurnEndDamageMarkerProgramHost
{
    public void AddEndingTurnDamageMarker(ProgramSkillFrame frame, PlayerMarkerKind marker) => getCoreGameEngineTurnEndDamageMarkerRuntime().Value.AddProgramEndingTurnDamageMarker(frame, marker);
}
