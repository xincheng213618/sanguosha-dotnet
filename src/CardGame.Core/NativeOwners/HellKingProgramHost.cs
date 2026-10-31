#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HellKingProgramHost(Func<RuntimeEntries.CoreHellKingProgramsEntries> getCoreHellKingProgramsRuntime) : IHellKingProgramHost
{
    public SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreHellKingProgramsRuntime().ExecuteHellKing(frame, effect);
}
