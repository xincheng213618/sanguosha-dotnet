#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFeiOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime> _resolve;
    private CharacterZhangFeiOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiOlMagicRuntime> resolve) => new(resolve);
}
