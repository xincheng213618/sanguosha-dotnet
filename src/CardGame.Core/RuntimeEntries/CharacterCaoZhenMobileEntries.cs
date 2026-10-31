#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhenMobileRuntime> _resolve;
    private CharacterCaoZhenMobileEntries(Func<EngineOwners.CharacterCaoZhenMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenMobileEntries Create(Func<EngineOwners.CharacterCaoZhenMobileRuntime> resolve) => new(resolve);
}
