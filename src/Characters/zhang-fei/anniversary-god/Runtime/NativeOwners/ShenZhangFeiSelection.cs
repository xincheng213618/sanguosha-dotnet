#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhangFeiSelection(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime) : IShenZhangFeiSelection
{
    public int Limit(int owner) => getCharacterZhangFeiAnniversaryGodRuntime().ShenZfLimit(owner);
}
