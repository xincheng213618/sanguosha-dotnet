#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileEntries
{
    private readonly Func<EngineOwners.CharacterChenQunMobileRuntime> _resolve;
    private CharacterChenQunMobileEntries(Func<EngineOwners.CharacterChenQunMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunMobileEntries Create(Func<EngineOwners.CharacterChenQunMobileRuntime> resolve) => new(resolve);
}
