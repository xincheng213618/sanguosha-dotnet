#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryBuLianShiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterBuLianShiBoundaryMobileRuntime>> getCharacterBuLianShiBoundaryMobileRuntime) : IBoundaryBuLianShiHost
{
    public void Prepare(ProgramSkillFrame f) => getCharacterBuLianShiBoundaryMobileRuntime().Value.PrepareBoundaryBuLianShiAnxu(f);
    public void SelectLeast(ProgramSkillFrame f) => getCharacterBuLianShiBoundaryMobileRuntime().Value.SelectBoundaryBuLianShiAnxuLeast(f);
}
