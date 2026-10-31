#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoRenBoundaryEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDiscardHandOrUseEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginDiscardHandOrUseEquipment(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMoveFieldEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginMoveFieldEquipment(frame);
    internal bool CanPayJushouHandCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().CanPayJushouHandCard(owner, card);
    internal void ResolveDiscardHandOrUseEquipment(global::CardGame.Core.ProgramSkillFrame frame, int cardId) => _resolve().ResolveDiscardHandOrUseEquipment(frame, cardId);
    internal void ResolveMoveFieldEquipment(global::CardGame.Core.ProgramSkillFrame frame, int cardId) => _resolve().ResolveMoveFieldEquipment(frame, cardId);
}
