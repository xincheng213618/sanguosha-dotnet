#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreEndingHandCountProgramsEntries
{
    internal void RecordProgramEndingHandCount(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker) => _resolve().RecordProgramEndingHandCount(frame, stateId, marker);
}
