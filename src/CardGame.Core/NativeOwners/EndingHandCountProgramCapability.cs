#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EndingHandCountProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreEndingHandCountProgramsRuntime>> getCoreEndingHandCountProgramsRuntime) : IEndingHandCountProgramHost
{
    public void RecordEndHandCountAndGrantMarker(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker) => getCoreEndingHandCountProgramsRuntime().Value.RecordProgramEndingHandCount(frame, stateId, marker);
}
