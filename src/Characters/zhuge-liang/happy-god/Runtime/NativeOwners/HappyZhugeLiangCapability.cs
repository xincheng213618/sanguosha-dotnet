#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhugeLiangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangHappyGodEntries> getCharacterZhugeLiangHappyGodRuntime) : IHappyZhugeLiangHost
{
    public void GrantMist(ProgramSkillFrame frame) => getCharacterZhugeLiangHappyGodRuntime().GrantHappyDawu(frame);
}
