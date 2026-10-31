#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class VirtualOrdinaryTrickProgramCapability(Func<RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime) : IVirtualOrdinaryTrickProgramHost
{
    public SkillProgramStepOutcome UseVirtualOrdinaryTrick(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineVirtualOrdinaryTrickUsesRuntime().BeginProgramVirtualOrdinaryTrick(frame, effect);
}
