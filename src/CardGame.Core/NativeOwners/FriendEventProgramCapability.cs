#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FriendEventProgramCapability(Func<RuntimeEntries.CoreGameEngineFriendEventsEntries> getCoreGameEngineFriendEventsRuntime) : IFriendEventProgramHost
{
    public SkillProgramStepOutcome ExecuteFriendEvent(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineFriendEventsRuntime().BeginFriendEvent(frame, operation);
}
