#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedEquipmentDrawsEntries
{
    internal SkillProgramStepOutcome BeginRoundGainedEquipmentDraw(ProgramSkillFrame supplied) => _resolve().BeginRoundGainedEquipmentDraw(supplied);
    internal bool CanOfferRoundGainedEquipmentDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferRoundGainedEquipmentDraw(candidate, trigger, context);
    internal RoundGainedUseQualification? ExactRoundGainedEquipmentDrawParent(ProgramSkillFrame frame) => _resolve().ExactRoundGainedEquipmentDrawParent(frame);
    internal bool ResumeRoundGainedEquipmentDraw(long id) => _resolve().ResumeRoundGainedEquipmentDraw(id);
    internal bool ReturnRoundGainedEquipmentDrawMovement(ProgramSkillFrame frame) => _resolve().ReturnRoundGainedEquipmentDrawMovement(frame);
    internal bool ValidRoundGainedEquipmentDraw(ProgramSkillFrame frame) => _resolve().ValidRoundGainedEquipmentDraw(frame);
}
