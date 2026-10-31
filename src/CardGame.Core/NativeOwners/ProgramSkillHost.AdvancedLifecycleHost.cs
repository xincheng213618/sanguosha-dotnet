#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome ExecuteAdvancedLifecycle(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineAdvancedLifecycleRuntime().ExecuteAdvancedLifecycle(effect, frame, targetSeat);
}
