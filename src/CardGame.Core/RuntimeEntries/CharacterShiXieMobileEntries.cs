#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterShiXieMobileEntries
{
    private readonly Func<EngineOwners.CharacterShiXieMobileRuntime> _resolve;
    private CharacterShiXieMobileEntries(Func<EngineOwners.CharacterShiXieMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterShiXieMobileEntries Create(Func<EngineOwners.CharacterShiXieMobileRuntime> resolve) => new(resolve);
}
