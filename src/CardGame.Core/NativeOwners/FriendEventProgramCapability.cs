#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FriendEventProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendEventsEntries> getCoreGameEngineFriendEventsRuntime) : IFriendEventProgramHost
{
    public SkillProgramStepOutcome ExecuteFriendEvent(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineFriendEventsRuntime().BeginFriendEvent(frame, operation);
}
