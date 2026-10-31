#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> _resolve;
    private CharacterXunYuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunYuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> resolve) => new(resolve);
}
