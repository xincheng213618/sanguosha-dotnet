#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiQuery(Func<RuntimeEntries.CharacterZhenJiHappyGodEntries> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiQuery
{
    public bool Allows(int owner, int target) => getCharacterZhenJiHappyGodRuntime().HappyShenfuTarget(owner, target);
}
