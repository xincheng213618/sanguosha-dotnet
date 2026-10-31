#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> _resolve;
    private CharacterGuoHuaiBoundaryMobileEntries(Func<EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiBoundaryMobileEntries Create(Func<EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> resolve) => new(resolve);
}
