#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanMobileRuntime> _resolve;
    private CharacterZhuHuanMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuHuanMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanMobileRuntime> resolve) => new(resolve);
}
