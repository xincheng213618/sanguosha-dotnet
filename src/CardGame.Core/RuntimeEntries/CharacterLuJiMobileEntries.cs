#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuJiMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuJiMobileRuntime> _resolve;
    private CharacterLuJiMobileEntries(Func<EngineOwners.CharacterLuJiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuJiMobileEntries Create(Func<EngineOwners.CharacterLuJiMobileRuntime> resolve) => new(resolve);
}
