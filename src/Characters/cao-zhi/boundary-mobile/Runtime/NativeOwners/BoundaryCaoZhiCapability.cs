#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhiBoundaryMobileRuntime>> getCharacterCaoZhiBoundaryMobileRuntime) : IBoundaryCaoZhiHost
{
    public void SelectTrick(ProgramSkillFrame frame, string bind) => getCharacterCaoZhiBoundaryMobileRuntime().Value.SelectBoundaryCaoZhiTrick(frame, bind);
    public void Upgrade(ProgramSkillFrame frame) => getCharacterCaoZhiBoundaryMobileRuntime().Value.UpgradeBoundaryCaoZhiJiushi(frame);
}
