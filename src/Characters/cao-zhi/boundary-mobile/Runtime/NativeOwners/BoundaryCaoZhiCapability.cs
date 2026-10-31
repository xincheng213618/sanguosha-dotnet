#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhiCapability(Func<RuntimeEntries.CharacterCaoZhiBoundaryMobileEntries> getCharacterCaoZhiBoundaryMobileRuntime) : IBoundaryCaoZhiHost
{
    public void SelectTrick(ProgramSkillFrame frame, string bind) => getCharacterCaoZhiBoundaryMobileRuntime().SelectBoundaryCaoZhiTrick(frame, bind);
    public void Upgrade(ProgramSkillFrame frame) => getCharacterCaoZhiBoundaryMobileRuntime().UpgradeBoundaryCaoZhiJiushi(frame);
}
