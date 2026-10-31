#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuLianShiMobileEntries
{
    internal bool KeepMobileBuLianShiAnxuPaid(ProgramSkillFrame f) => _resolve().KeepMobileBuLianShiAnxuPaid(f);
    internal bool MobileBuLianShiAnxuPaid(ProgramSkillFrame f) => _resolve().MobileBuLianShiAnxuPaid(f);
    internal void PrepareMobileBuLianShiAnxu(ProgramSkillFrame supplied) => _resolve().PrepareMobileBuLianShiAnxu(supplied);
    internal void RecordMobileBuLianShiAnxuDelivery(CardMovedEvent fact) => _resolve().RecordMobileBuLianShiAnxuDelivery(fact);
    internal bool ReturnMobileBuLianShiAnxuPaid(ProgramSkillFrame f) => _resolve().ReturnMobileBuLianShiAnxuPaid(f);
}
