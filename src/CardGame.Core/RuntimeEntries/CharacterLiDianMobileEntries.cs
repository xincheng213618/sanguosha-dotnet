#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiDianMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiDianMobileRuntime> _resolve;
    private CharacterLiDianMobileEntries(Func<EngineOwners.CharacterLiDianMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiDianMobileEntries Create(Func<EngineOwners.CharacterLiDianMobileRuntime> resolve) => new(resolve);
}
