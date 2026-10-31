#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoZhenBoundaryMobileRuntime))]
internal sealed partial class CharacterCaoZhenBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> _resolve;
    private CharacterCaoZhenBoundaryMobileEntries(Func<EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> resolve) => new(resolve);
}
