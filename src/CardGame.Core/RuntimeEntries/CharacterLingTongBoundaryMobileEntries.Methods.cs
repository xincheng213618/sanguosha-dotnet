#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLingTongBoundaryMobileEntries
{
    internal void AnswerBoundaryLingTongEquipmentPair(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryLingTongEquipmentPair(choice);
    internal global::System.Collections.Generic.IEnumerable<(int Source, int Destination)> BoundaryLingTongEquipmentPairs(int ownerSeat) => _resolve().BoundaryLingTongEquipmentPairs(ownerSeat);
    internal bool QualifyBoundaryLingTongXuanfeng(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().QualifyBoundaryLingTongXuanfeng(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectBoundaryLingTongEquipmentPair(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectBoundaryLingTongEquipmentPair(frame);
    internal bool ValidBoundaryLingTongEquipmentSelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryLingTongEquipmentSelection(f);
}
