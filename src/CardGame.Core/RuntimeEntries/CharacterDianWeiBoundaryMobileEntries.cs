#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterDianWeiBoundaryMobileRuntime> _resolve;
    private CharacterDianWeiBoundaryMobileEntries(Func<EngineOwners.CharacterDianWeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiBoundaryMobileEntries Create(Func<EngineOwners.CharacterDianWeiBoundaryMobileRuntime> resolve) => new(resolve);
}
