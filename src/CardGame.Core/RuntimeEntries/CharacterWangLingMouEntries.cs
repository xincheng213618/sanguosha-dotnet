#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangLingMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangLingMouRuntime> _resolve;
    private CharacterWangLingMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangLingMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangLingMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangLingMouRuntime> resolve) => new(resolve);
}
