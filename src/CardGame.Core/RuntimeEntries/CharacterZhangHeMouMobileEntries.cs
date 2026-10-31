#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHeMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHeMouMobileRuntime> _resolve;
    private CharacterZhangHeMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHeMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHeMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHeMouMobileRuntime> resolve) => new(resolve);
}
