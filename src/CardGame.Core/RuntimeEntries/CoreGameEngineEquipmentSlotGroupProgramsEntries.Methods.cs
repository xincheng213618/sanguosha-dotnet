#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSlotGroupProgramsEntries
{
    internal void AbolishProgramEquipmentSlotGroup(ProgramSkillFrame frame, IReadOnlyList<EquipmentSlot> slots) => _resolve().AbolishProgramEquipmentSlotGroup(frame, slots);
    internal bool CanBePindianTarget(int sourceSeat, int targetSeat) => _resolve().CanBePindianTarget(sourceSeat, targetSeat);
    internal bool CanPayEquipmentSlotGroup(CharacterState owner, SkillProgramActivation activation) => _resolve().CanPayEquipmentSlotGroup(owner, activation);
    internal bool CanSelectProgramPindianOpponent(ProgramSkillFrame frame, int targetSeat) => _resolve().CanSelectProgramPindianOpponent(frame, targetSeat);
    internal ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat) => _resolve().CreateProgramLegalityParticipant(seat);
    internal void RecastProgramSelectedEquipment(ProgramSkillFrame frame) => _resolve().RecastProgramSelectedEquipment(frame);
    internal void ReplaceProgramPreparationSkills(ProgramSkillFrame frame, IReadOnlyList<string> lost, string acquired) => _resolve().ReplaceProgramPreparationSkills(frame, lost, acquired);
    internal bool ResolveEnteringChain(CharacterState player, bool requested) => _resolve().ResolveEnteringChain(player, requested);
}
