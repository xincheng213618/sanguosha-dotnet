#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> _resolve;
    private CharacterHuaTuoBoundaryMobileEntries(Func<EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoBoundaryMobileEntries Create(Func<EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> resolve) => new(resolve);
}
