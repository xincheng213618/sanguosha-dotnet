#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYingOlRuntime> _resolve;
    private CharacterCaoYingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoYingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYingOlRuntime> resolve) => new(resolve);
}
