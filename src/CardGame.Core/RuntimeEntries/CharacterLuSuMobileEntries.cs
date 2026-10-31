#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuSuMobileRuntime> _resolve;
    private CharacterLuSuMobileEntries(Func<EngineOwners.CharacterLuSuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileEntries Create(Func<EngineOwners.CharacterLuSuMobileRuntime> resolve) => new(resolve);
}
