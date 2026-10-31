#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoRenBoundaryEntries
{
    internal SkillProgramStepOutcome BeginDiscardHandOrUseEquipment(ProgramSkillFrame frame) => _resolve().BeginDiscardHandOrUseEquipment(frame);
    internal SkillProgramStepOutcome BeginMoveFieldEquipment(ProgramSkillFrame frame) => _resolve().BeginMoveFieldEquipment(frame);
    internal bool CanPayJushouHandCard(CharacterState owner, Card card) => _resolve().CanPayJushouHandCard(owner, card);
    internal void ResolveDiscardHandOrUseEquipment(ProgramSkillFrame frame, int cardId) => _resolve().ResolveDiscardHandOrUseEquipment(frame, cardId);
    internal void ResolveMoveFieldEquipment(ProgramSkillFrame frame, int cardId) => _resolve().ResolveMoveFieldEquipment(frame, cardId);
}
