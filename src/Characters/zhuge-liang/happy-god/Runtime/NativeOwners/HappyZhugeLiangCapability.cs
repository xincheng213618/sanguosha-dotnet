#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhugeLiangCapability(Func<RuntimeEntries.CharacterZhugeLiangHappyGodEntries> getCharacterZhugeLiangHappyGodRuntime) : IHappyZhugeLiangHost
{
    public void GrantMist(ProgramSkillFrame frame) => getCharacterZhugeLiangHappyGodRuntime().GrantHappyDawu(frame);
}
