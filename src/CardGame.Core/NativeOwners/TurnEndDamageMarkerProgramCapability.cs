#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnEndDamageMarkerProgramCapability(Func<RuntimeEntries.CoreGameEngineTurnEndDamageMarkerEntries> getCoreGameEngineTurnEndDamageMarkerRuntime) : ITurnEndDamageMarkerProgramHost
{
    public void AddEndingTurnDamageMarker(ProgramSkillFrame frame, PlayerMarkerKind marker) => getCoreGameEngineTurnEndDamageMarkerRuntime().AddProgramEndingTurnDamageMarker(frame, marker);
}
