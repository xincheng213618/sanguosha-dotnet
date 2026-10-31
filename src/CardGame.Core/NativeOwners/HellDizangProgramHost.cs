#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HellDizangProgramHost(Func<RuntimeEntries.CoreHellDizangProgramsEntries> getCoreHellDizangProgramsRuntime) : IHellDizangProgramHost
{
    public SkillProgramStepOutcome ExecuteHellDizang(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreHellDizangProgramsRuntime().ExecuteHellDizang(frame, operation);
}
