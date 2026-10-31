#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYanMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiYanMobileRuntime> _resolve;
    private CharacterLiYanMobileEntries(Func<EngineOwners.CharacterLiYanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiYanMobileEntries Create(Func<EngineOwners.CharacterLiYanMobileRuntime> resolve) => new(resolve);
}
