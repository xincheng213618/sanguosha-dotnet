#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLingTongBoundaryMobileEntries
{
    internal void AnswerBoundaryLingTongEquipmentPair(PromptChoice choice) => _resolve().AnswerBoundaryLingTongEquipmentPair(choice);
    internal IEnumerable<(int Source, int Destination)> BoundaryLingTongEquipmentPairs(int ownerSeat) => _resolve().BoundaryLingTongEquipmentPairs(ownerSeat);
    internal bool QualifyBoundaryLingTongXuanfeng(ProgramTriggerLegalityContext gate) => _resolve().QualifyBoundaryLingTongXuanfeng(gate);
    internal SkillProgramStepOutcome SelectBoundaryLingTongEquipmentPair(ProgramSkillFrame frame) => _resolve().SelectBoundaryLingTongEquipmentPair(frame);
}
