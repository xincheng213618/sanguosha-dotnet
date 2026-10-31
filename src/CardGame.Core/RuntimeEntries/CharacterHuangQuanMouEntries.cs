#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> _resolve;
    private CharacterHuangQuanMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> resolve) => new(resolve);
}
