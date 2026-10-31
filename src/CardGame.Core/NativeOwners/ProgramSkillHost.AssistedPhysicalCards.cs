#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome RequestSlashAgainstChosenTarget(ProgramSkillFrame frame, int actorSeat, string resultBind, bool actorChoosesTarget = false) => getCoreGameEngineAssistedPhysicalCardsRuntime().RequestProgramSlashAgainstChosenTarget(frame, actorSeat, resultBind, actorChoosesTarget);
    public SkillProgramStepOutcome TakeSelectedTargetCards(ProgramSkillFrame frame, int sourceSeat, int count) => getCoreGameEngineAssistedPhysicalCardsRuntime().TakeProgramSelectedTargetCards(frame, sourceSeat, count);
}
