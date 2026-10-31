#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _resolve;
    private CharacterSimaYiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> resolve) => new(resolve);
}
