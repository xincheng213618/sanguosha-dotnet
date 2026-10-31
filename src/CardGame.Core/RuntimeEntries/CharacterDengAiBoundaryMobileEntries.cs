#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterDengAiBoundaryMobileRuntime> _resolve;
    private CharacterDengAiBoundaryMobileEntries(Func<EngineOwners.CharacterDengAiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiBoundaryMobileEntries Create(Func<EngineOwners.CharacterDengAiBoundaryMobileRuntime> resolve) => new(resolve);
}
