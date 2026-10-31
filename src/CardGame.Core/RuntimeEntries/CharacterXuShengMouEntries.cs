#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShengMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShengMouRuntime> _resolve;
    private CharacterXuShengMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuShengMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShengMouRuntime> resolve) => new(resolve);
}
