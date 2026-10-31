#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSlotGroupProgramsEntries
{
    internal void AbolishProgramEquipmentSlotGroup(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EquipmentSlot> slots) => _resolve().AbolishProgramEquipmentSlotGroup(frame, slots);
    internal bool CanBePindianTarget(int sourceSeat, int targetSeat) => _resolve().CanBePindianTarget(sourceSeat, targetSeat);
    internal bool CanPayEquipmentSlotGroup(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanPayEquipmentSlotGroup(owner, activation);
    internal bool CanSelectProgramPindianOpponent(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().CanSelectProgramPindianOpponent(frame, targetSeat);
    internal global::CardGame.Core.ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat) => _resolve().CreateProgramLegalityParticipant(seat);
    internal void RecastProgramSelectedEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RecastProgramSelectedEquipment(frame);
    internal void ReplaceProgramPreparationSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> lost, string acquired) => _resolve().ReplaceProgramPreparationSkills(frame, lost, acquired);
    internal bool ResolveEnteringChain(global::CardGame.Core.CharacterState player, bool requested) => _resolve().ResolveEnteringChain(player, requested);
}
