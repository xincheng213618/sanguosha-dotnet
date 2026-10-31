#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuLianShiBoundaryMobileEntries
{
    internal void CaptureBoundaryBuLianShiAnxuObtain(CardMovedEvent fact) => _resolve().CaptureBoundaryBuLianShiAnxuObtain(fact);
    internal bool KeepIssuedBoundaryBuLianShiAnxu(ProgramSkillFrame f) => _resolve().KeepIssuedBoundaryBuLianShiAnxu(f);
    internal void PrepareBoundaryBuLianShiAnxu(ProgramSkillFrame f) => _resolve().PrepareBoundaryBuLianShiAnxu(f);
    internal void SelectBoundaryBuLianShiAnxuLeast(ProgramSkillFrame f) => _resolve().SelectBoundaryBuLianShiAnxuLeast(f);
    internal bool ValidBoundaryBuLianShiAnxu(ProgramSkillFrame f) => _resolve().ValidBoundaryBuLianShiAnxu(f);
}
