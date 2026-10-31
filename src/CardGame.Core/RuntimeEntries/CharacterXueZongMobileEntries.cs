#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXueZongMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXueZongMobileRuntime> _resolve;
    private CharacterXueZongMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXueZongMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXueZongMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXueZongMobileRuntime> resolve) => new(resolve);
}
