#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiQuery(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhenJiHappyGodRuntime>> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiQuery
{
    public bool Allows(int owner, int target) => getCharacterZhenJiHappyGodRuntime().Value.HappyShenfuTarget(owner, target);
}
