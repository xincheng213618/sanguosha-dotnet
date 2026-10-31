#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class FriendTrioProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries> getCoreGameEngineFriendTrioRuntime) : IFriendTrioProgramHost
{
    public SkillProgramStepOutcome ExecuteFriendTrio(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineFriendTrioRuntime().ExecuteFriendTrio(f, op);
}
