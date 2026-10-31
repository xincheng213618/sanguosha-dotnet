#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouRuntime> _resolve;
    private CharacterHuangGaiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangGaiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouRuntime> resolve) => new(resolve);
}
