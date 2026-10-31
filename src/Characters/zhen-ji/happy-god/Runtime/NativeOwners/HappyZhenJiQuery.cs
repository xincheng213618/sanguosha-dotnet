#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhenJiHappyGodEntries> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiQuery
{
    public bool Allows(int owner, int target) => getCharacterZhenJiHappyGodRuntime().HappyShenfuTarget(owner, target);
}
