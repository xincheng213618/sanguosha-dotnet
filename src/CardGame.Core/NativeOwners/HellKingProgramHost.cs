#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HellKingProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreHellKingProgramsRuntime>> getCoreHellKingProgramsRuntime) : IHellKingProgramHost
{
    public SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreHellKingProgramsRuntime().Value.ExecuteHellKing(frame, effect);
}
