#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaHuiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> _resolve;
    private CharacterSimaHuiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaHuiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> resolve) => new(resolve);
}
