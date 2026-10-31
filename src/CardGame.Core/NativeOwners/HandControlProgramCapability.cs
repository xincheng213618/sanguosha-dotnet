#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandControlProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramHandControlRuntime>> getCoreGameEngineProgramHandControlRuntime) : IHandControlProgramHost
{
    public SkillProgramStepOutcome ExecuteHandControl(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramHandControlRuntime().Value.ExecuteProgramHandControl(effect, frame);
}
