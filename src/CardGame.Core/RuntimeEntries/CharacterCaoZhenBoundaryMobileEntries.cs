#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime))]
internal sealed partial class CharacterCaoZhenBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> _resolve;
    private CharacterCaoZhenBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> resolve) => new(resolve);
}
