#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class HellDizangProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellDizangProgramsEntries> getCoreHellDizangProgramsRuntime) : IHellDizangProgramHost
{
    public SkillProgramStepOutcome ExecuteHellDizang(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreHellDizangProgramsRuntime().ExecuteHellDizang(frame, operation);
}
