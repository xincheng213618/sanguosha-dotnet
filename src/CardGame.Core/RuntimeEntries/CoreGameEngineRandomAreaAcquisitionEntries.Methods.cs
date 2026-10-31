#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionEntries
{
    internal bool CanContinueRandomAreaAcquisition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueRandomAreaAcquisition(frame);
    internal bool DrainRandomAreaChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainRandomAreaChildren(frame);
    internal bool ExactRandomAreaParent(global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.DamageTriggerWindowFrame window, scoped out global::CardGame.Core.DamageFrame damage) => _resolve().ExactRandomAreaParent(frame, out window, out damage);
    internal void FinishRandomAreaAcquisition(global::CardGame.Core.ProgramSkillFrame frame, bool cancelled) => _resolve().FinishRandomAreaAcquisition(frame, cancelled);
    internal bool IsRandomAreaAcquisitionMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsRandomAreaAcquisitionMovement(frame, effect, pending);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainRandomAreaFromEachOtherThenTurnOver(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ObtainRandomAreaFromEachOtherThenTurnOver(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> RandomAreaCards(int seat, global::CardGame.Core.CardZoneKind zone) => _resolve().RandomAreaCards(seat, zone);
    internal bool ResumeRandomAreaAcquisition(long id) => _resolve().ResumeRandomAreaAcquisition(id);
    internal bool ReturnRandomAreaMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnRandomAreaMovement(frame);
}
