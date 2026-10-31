#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHeMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhangHeMouMobileRuntime> _resolve;
    private CharacterZhangHeMouMobileEntries(Func<EngineOwners.CharacterZhangHeMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHeMouMobileEntries Create(Func<EngineOwners.CharacterZhangHeMouMobileRuntime> resolve) => new(resolve);
}
