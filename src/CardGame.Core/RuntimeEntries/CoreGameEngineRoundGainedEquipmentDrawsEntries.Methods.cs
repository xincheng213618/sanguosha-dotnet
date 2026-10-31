#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedEquipmentDrawsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRoundGainedEquipmentDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginRoundGainedEquipmentDraw(supplied);
    internal bool CanOfferRoundGainedEquipmentDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRoundGainedEquipmentDraw(candidate, trigger, context);
    internal global::CardGame.Core.RoundGainedUseQualification? ExactRoundGainedEquipmentDrawParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactRoundGainedEquipmentDrawParent(frame);
    internal bool IsRoundGainedEquipmentDrawAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsRoundGainedEquipmentDrawAwaitedMovement(frame, effect, pending);
    internal bool ResumeRoundGainedEquipmentDraw(long id) => _resolve().ResumeRoundGainedEquipmentDraw(id);
    internal bool ReturnRoundGainedEquipmentDrawMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnRoundGainedEquipmentDrawMovement(frame);
    internal bool ValidRoundGainedEquipmentDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidRoundGainedEquipmentDraw(frame);
}
