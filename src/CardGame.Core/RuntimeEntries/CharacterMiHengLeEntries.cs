#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMiHengLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _resolve;
    private CharacterMiHengLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMiHengLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> resolve) => new(resolve);
}
