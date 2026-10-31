#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuFuAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> _resolve;
    private CharacterWuFuAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuFuAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> resolve) => new(resolve);
}
