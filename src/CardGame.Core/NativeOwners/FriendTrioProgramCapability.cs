#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FriendTrioProgramCapability(Func<RuntimeEntries.CoreGameEngineFriendTrioEntries> getCoreGameEngineFriendTrioRuntime) : IFriendTrioProgramHost
{
    public SkillProgramStepOutcome ExecuteFriendTrio(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineFriendTrioRuntime().ExecuteFriendTrio(f, op);
}
