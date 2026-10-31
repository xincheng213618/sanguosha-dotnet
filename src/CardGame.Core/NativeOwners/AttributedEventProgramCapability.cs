#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AttributedEventProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries> getCoreGameEngineAttributedMarkerEventsRuntime) : IAttributedEventProgramHost
{
    public void ApplyAttributedEvent(ProgramSkillFrame f, SkillProgramEffect e, int target) => getCoreGameEngineAttributedMarkerEventsRuntime().ApplyAttributedEvent(f, e, target);
}
