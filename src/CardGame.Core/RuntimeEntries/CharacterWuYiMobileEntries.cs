#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuYiMobileEntries
{
    private readonly Func<EngineOwners.CharacterWuYiMobileRuntime> _resolve;
    private CharacterWuYiMobileEntries(Func<EngineOwners.CharacterWuYiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuYiMobileEntries Create(Func<EngineOwners.CharacterWuYiMobileRuntime> resolve) => new(resolve);
}
