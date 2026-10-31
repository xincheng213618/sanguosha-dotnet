#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXueZongMobileEntries
{
    private readonly Func<EngineOwners.CharacterXueZongMobileRuntime> _resolve;
    private CharacterXueZongMobileEntries(Func<EngineOwners.CharacterXueZongMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXueZongMobileEntries Create(Func<EngineOwners.CharacterXueZongMobileRuntime> resolve) => new(resolve);
}
