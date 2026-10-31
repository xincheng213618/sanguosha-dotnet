#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandControlProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramHandControlEntries> getCoreGameEngineProgramHandControlRuntime) : IHandControlProgramHost
{
    public SkillProgramStepOutcome ExecuteHandControl(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramHandControlRuntime().ExecuteProgramHandControl(effect, frame);
}
