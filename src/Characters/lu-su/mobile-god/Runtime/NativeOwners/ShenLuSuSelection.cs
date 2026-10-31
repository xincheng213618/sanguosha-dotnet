#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenLuSuSelection(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileGodEntries> getCharacterLuSuMobileGodRuntime) : IShenLuSuSelection
{
    public bool DingzhouTarget(int owner, int target) => getCharacterLuSuMobileGodRuntime().ShenLsDingzhouTarget(owner, target);
}
