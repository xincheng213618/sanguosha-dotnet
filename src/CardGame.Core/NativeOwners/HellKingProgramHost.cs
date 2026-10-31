#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HellKingProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries> getCoreHellKingProgramsRuntime) : IHellKingProgramHost
{
    public SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreHellKingProgramsRuntime().ExecuteHellKing(frame, effect);
}
