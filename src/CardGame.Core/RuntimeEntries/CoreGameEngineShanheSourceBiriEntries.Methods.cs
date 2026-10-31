#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceBiriEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheSourceBiri(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheSourceBiri(supplied);
    internal bool CanFinishShanheSourceBiri(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanFinishShanheSourceBiri(frame);
    internal bool CanOfferShanheSourceBiri(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferShanheSourceBiri(context);
    internal void CaptureShanheSourceBiriRecovery(global::CardGame.Core.RecoveryAppliedEvent applied) => _resolve().CaptureShanheSourceBiriRecovery(applied);
    internal void ContinueShanheSourceBiri(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinueShanheSourceBiri(supplied);
    internal global::CardGame.Core.BeforeDamageProgramWindowFrame ExactShanheSourceBiriParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactShanheSourceBiriParent(frame);
    internal bool ResumeShanheSourceBiri(long id) => _resolve().ResumeShanheSourceBiri(id);
    internal bool ReturnShanheSourceBiriMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheSourceBiriMovement(frame);
}
