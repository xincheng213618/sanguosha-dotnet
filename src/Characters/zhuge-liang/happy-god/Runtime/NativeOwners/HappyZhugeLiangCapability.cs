#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhugeLiangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhugeLiangHappyGodRuntime>> getCharacterZhugeLiangHappyGodRuntime) : IHappyZhugeLiangHost
{
    public void GrantMist(ProgramSkillFrame frame) => getCharacterZhugeLiangHappyGodRuntime().Value.GrantHappyDawu(frame);
}
