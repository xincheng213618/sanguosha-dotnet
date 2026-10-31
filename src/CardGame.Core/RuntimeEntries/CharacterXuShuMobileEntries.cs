#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShuMobileEntries
{
    private readonly Func<EngineOwners.CharacterXuShuMobileRuntime> _resolve;
    private CharacterXuShuMobileEntries(Func<EngineOwners.CharacterXuShuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuShuMobileEntries Create(Func<EngineOwners.CharacterXuShuMobileRuntime> resolve) => new(resolve);
}
