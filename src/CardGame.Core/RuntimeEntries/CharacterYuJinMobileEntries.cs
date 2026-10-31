#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMobileEntries
{
    private readonly Func<EngineOwners.CharacterYuJinMobileRuntime> _resolve;
    private CharacterYuJinMobileEntries(Func<EngineOwners.CharacterYuJinMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJinMobileEntries Create(Func<EngineOwners.CharacterYuJinMobileRuntime> resolve) => new(resolve);
}
