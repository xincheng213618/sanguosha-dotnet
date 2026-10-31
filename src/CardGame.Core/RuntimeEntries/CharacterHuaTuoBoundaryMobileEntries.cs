#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> _resolve;
    private CharacterHuaTuoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoBoundaryMobileRuntime> resolve) => new(resolve);
}
