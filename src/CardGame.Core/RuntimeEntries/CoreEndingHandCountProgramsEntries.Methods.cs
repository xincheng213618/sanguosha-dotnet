#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreEndingHandCountProgramsEntries
{
    internal void RecordProgramEndingHandCount(global::CardGame.Core.ProgramSkillFrame frame, string stateId, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().RecordProgramEndingHandCount(frame, stateId, marker);
}
