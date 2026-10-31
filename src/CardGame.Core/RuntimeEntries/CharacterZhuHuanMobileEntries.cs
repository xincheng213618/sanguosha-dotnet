#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuHuanMobileRuntime> _resolve;
    private CharacterZhuHuanMobileEntries(Func<EngineOwners.CharacterZhuHuanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuHuanMobileEntries Create(Func<EngineOwners.CharacterZhuHuanMobileRuntime> resolve) => new(resolve);
}
