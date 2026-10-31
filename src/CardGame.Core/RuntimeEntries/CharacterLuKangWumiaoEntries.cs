#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuKangWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _resolve;
    private CharacterLuKangWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuKangWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> resolve) => new(resolve);
}
