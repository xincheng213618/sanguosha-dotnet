#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXuChuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXuChuBoundaryMobileRuntime>> getCharacterXuChuBoundaryMobileRuntime) : IBoundaryXuChuHost
{
    public void SubstituteBoundaryXuChuDraw(ProgramSkillFrame f) => getCharacterXuChuBoundaryMobileRuntime().Value.SubstituteBoundaryXuChuDraw(f);
}
