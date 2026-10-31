#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _resolve;
    private CharacterLuMengMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> resolve) => new(resolve);
}
