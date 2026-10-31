#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhangFeiSelection(Func<RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime) : IShenZhangFeiSelection
{
    public int Limit(int owner) => getCharacterZhangFeiAnniversaryGodRuntime().ShenZfLimit(owner);
}
