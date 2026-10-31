#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMobilizationEntries
{
    internal void AssertShanheMobilization(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertShanheMobilization(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheMobilization(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheMobilization(supplied);
    internal bool CanCompleteShanheMobilization(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanCompleteShanheMobilization(frame);
    internal void ContinueShanheMobilization(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinueShanheMobilization(supplied);
    internal bool IsActualShanheMobilizationRound(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsActualShanheMobilizationRound(frame);
    internal bool ResumeShanheMobilization(long id) => _resolve().ResumeShanheMobilization(id);
    internal bool ReturnShanheMobilizationMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheMobilizationMovement(frame);
    internal int[] ShanheMobilizationDeadAllies(int ownerSeat) => _resolve().ShanheMobilizationDeadAllies(ownerSeat);
}
