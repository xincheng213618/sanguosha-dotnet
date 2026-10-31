#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuXunWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _resolve;
    private CharacterLuXunWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuXunWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> resolve) => new(resolve);
}
