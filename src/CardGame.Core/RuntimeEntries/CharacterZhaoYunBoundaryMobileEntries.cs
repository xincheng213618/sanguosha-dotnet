#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> _resolve;
    private CharacterZhaoYunBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> resolve) => new(resolve);
}
