#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class VirtualOrdinaryTrickProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries> getCoreGameEngineVirtualOrdinaryTrickUsesRuntime) : IVirtualOrdinaryTrickProgramHost
{
    public SkillProgramStepOutcome UseVirtualOrdinaryTrick(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineVirtualOrdinaryTrickUsesRuntime().BeginProgramVirtualOrdinaryTrick(frame, effect);
}
