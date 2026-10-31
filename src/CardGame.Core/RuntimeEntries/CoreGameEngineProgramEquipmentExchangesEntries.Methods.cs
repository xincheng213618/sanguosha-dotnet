#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEquipmentExchangesEntries
{
    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetEquipment(ProgramSkillFrame frame) => _resolve().ExchangeProgramSelectedTargetEquipment(frame);
    internal bool IsProgramEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat) => _resolve().IsProgramEquipmentExchangePair(ownerSeat, firstSeat, secondSeat);
}
