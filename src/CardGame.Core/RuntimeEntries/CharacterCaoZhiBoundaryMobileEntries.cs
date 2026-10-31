#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> _resolve;
    private CharacterCaoZhiBoundaryMobileEntries(Func<EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhiBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> resolve) => new(resolve);
}
