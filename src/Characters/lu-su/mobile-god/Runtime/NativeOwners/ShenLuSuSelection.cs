#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenLuSuSelection(Func<RuntimeEntries.CharacterLuSuMobileGodEntries> getCharacterLuSuMobileGodRuntime) : IShenLuSuSelection
{
    public bool DingzhouTarget(int owner, int target) => getCharacterLuSuMobileGodRuntime().ShenLsDingzhouTarget(owner, target);
}
