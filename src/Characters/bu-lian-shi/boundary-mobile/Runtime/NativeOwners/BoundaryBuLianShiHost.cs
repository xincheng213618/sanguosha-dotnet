#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryBuLianShiHost(Func<RuntimeEntries.CharacterBuLianShiBoundaryMobileEntries> getCharacterBuLianShiBoundaryMobileRuntime) : IBoundaryBuLianShiHost
{
    public void Prepare(ProgramSkillFrame f) => getCharacterBuLianShiBoundaryMobileRuntime().PrepareBoundaryBuLianShiAnxu(f);
    public void SelectLeast(ProgramSkillFrame f) => getCharacterBuLianShiBoundaryMobileRuntime().SelectBoundaryBuLianShiAnxuLeast(f);
}
