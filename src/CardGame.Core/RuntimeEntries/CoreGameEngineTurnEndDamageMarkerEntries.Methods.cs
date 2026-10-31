#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndDamageMarkerEntries
{
    internal void AddProgramEndingTurnDamageMarker(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().AddProgramEndingTurnDamageMarker(supplied, marker);
}
