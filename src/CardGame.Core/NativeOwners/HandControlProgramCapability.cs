#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandControlProgramCapability(Func<RuntimeEntries.CoreGameEngineProgramHandControlEntries> getCoreGameEngineProgramHandControlRuntime) : IHandControlProgramHost
{
    public SkillProgramStepOutcome ExecuteHandControl(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramHandControlRuntime().ExecuteProgramHandControl(effect, frame);
}
