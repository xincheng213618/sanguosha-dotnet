#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMobilizationEntries
{
    internal SkillProgramStepOutcome BeginShanheMobilization(ProgramSkillFrame supplied) => _resolve().BeginShanheMobilization(supplied);
    internal bool CanCompleteShanheMobilization(ProgramSkillFrame frame) => _resolve().CanCompleteShanheMobilization(frame);
    internal void ContinueShanheMobilization(ProgramSkillFrame supplied) => _resolve().ContinueShanheMobilization(supplied);
    internal bool IsActualShanheMobilizationRound(ProgramSkillFrame frame) => _resolve().IsActualShanheMobilizationRound(frame);
    internal bool ResumeShanheMobilization(long id) => _resolve().ResumeShanheMobilization(id);
    internal bool ReturnShanheMobilizationMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheMobilizationMovement(frame);
    internal int[] ShanheMobilizationDeadAllies(int ownerSeat) => _resolve().ShanheMobilizationDeadAllies(ownerSeat);
}
