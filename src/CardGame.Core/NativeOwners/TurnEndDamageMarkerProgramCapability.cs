#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnEndDamageMarkerProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnEndDamageMarkerEntries> getCoreGameEngineTurnEndDamageMarkerRuntime) : ITurnEndDamageMarkerProgramHost
{
    public void AddEndingTurnDamageMarker(ProgramSkillFrame frame, PlayerMarkerKind marker) => getCoreGameEngineTurnEndDamageMarkerRuntime().AddProgramEndingTurnDamageMarker(frame, marker);
}
