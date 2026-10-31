#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceBiriEntries
{
    internal SkillProgramStepOutcome BeginShanheSourceBiri(ProgramSkillFrame supplied) => _resolve().BeginShanheSourceBiri(supplied);
    internal bool CanFinishShanheSourceBiri(ProgramSkillFrame frame) => _resolve().CanFinishShanheSourceBiri(frame);
    internal bool CanOfferShanheSourceBiri(ProgramTriggerLegalityContext context) => _resolve().CanOfferShanheSourceBiri(context);
    internal void CaptureShanheSourceBiriRecovery(RecoveryAppliedEvent applied) => _resolve().CaptureShanheSourceBiriRecovery(applied);
    internal void ContinueShanheSourceBiri(ProgramSkillFrame supplied) => _resolve().ContinueShanheSourceBiri(supplied);
    internal BeforeDamageProgramWindowFrame ExactShanheSourceBiriParent(ProgramSkillFrame frame) => _resolve().ExactShanheSourceBiriParent(frame);
    internal bool ResumeShanheSourceBiri(long id) => _resolve().ResumeShanheSourceBiri(id);
    internal bool ReturnShanheSourceBiriMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheSourceBiriMovement(frame);
}
