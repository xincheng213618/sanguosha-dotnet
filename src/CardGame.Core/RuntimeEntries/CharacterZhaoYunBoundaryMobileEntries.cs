#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> _resolve;
    private CharacterZhaoYunBoundaryMobileEntries(Func<EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhaoYunBoundaryMobileRuntime> resolve) => new(resolve);
}
