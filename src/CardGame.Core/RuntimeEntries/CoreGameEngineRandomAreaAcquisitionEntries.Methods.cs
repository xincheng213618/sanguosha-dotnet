#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionEntries
{
    internal bool CanContinueRandomAreaAcquisition(ProgramSkillFrame frame) => _resolve().CanContinueRandomAreaAcquisition(frame);
    internal bool DrainRandomAreaChildren(ProgramSkillFrame frame) => _resolve().DrainRandomAreaChildren(frame);
    internal bool ExactRandomAreaParent(ProgramSkillFrame frame, scoped out DamageTriggerWindowFrame window, scoped out DamageFrame damage) => _resolve().ExactRandomAreaParent(frame, out window, out damage);
    internal void FinishRandomAreaAcquisition(ProgramSkillFrame frame, bool cancelled) => _resolve().FinishRandomAreaAcquisition(frame, cancelled);
    internal SkillProgramStepOutcome ObtainRandomAreaFromEachOtherThenTurnOver(ProgramSkillFrame supplied) => _resolve().ObtainRandomAreaFromEachOtherThenTurnOver(supplied);
    internal IReadOnlyList<Card> RandomAreaCards(int seat, CardZoneKind zone) => _resolve().RandomAreaCards(seat, zone);
    internal bool ResumeRandomAreaAcquisition(long id) => _resolve().ResumeRandomAreaAcquisition(id);
    internal bool ReturnRandomAreaMovement(ProgramSkillFrame frame) => _resolve().ReturnRandomAreaMovement(frame);
}
