#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEquipmentExchangesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ExchangeProgramSelectedTargetEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExchangeProgramSelectedTargetEquipment(frame);
    internal bool IsProgramEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat) => _resolve().IsProgramEquipmentExchangePair(ownerSeat, firstSeat, secondSeat);
}
