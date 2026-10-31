#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndDamageMarkerEntries
{
    internal void AddProgramEndingTurnDamageMarker(ProgramSkillFrame supplied, PlayerMarkerKind marker) => _resolve().AddProgramEndingTurnDamageMarker(supplied, marker);
}
