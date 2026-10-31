#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EndingHandCountProgramCapability(Func<RuntimeEntries.CoreEndingHandCountProgramsEntries> getCoreEndingHandCountProgramsRuntime) : IEndingHandCountProgramHost
{
    public void RecordEndHandCountAndGrantMarker(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker) => getCoreEndingHandCountProgramsRuntime().RecordProgramEndingHandCount(frame, stateId, marker);
}
