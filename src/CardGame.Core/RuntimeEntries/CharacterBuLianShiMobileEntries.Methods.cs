#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuLianShiMobileEntries
{
    internal bool KeepMobileBuLianShiAnxuPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileBuLianShiAnxuPaid(f);
    internal bool MobileBuLianShiAnxuPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileBuLianShiAnxuPaid(f);
    internal void PrepareMobileBuLianShiAnxu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareMobileBuLianShiAnxu(supplied);
    internal void RecordMobileBuLianShiAnxuDelivery(global::CardGame.Core.CardMovedEvent fact) => _resolve().RecordMobileBuLianShiAnxuDelivery(fact);
    internal bool ReturnMobileBuLianShiAnxuPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileBuLianShiAnxuPaid(f);
}
