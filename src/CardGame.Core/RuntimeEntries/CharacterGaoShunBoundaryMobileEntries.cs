#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGaoShunBoundaryMobileRuntime))]
internal sealed partial class CharacterGaoShunBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterGaoShunBoundaryMobileRuntime> _resolve;
    private CharacterGaoShunBoundaryMobileEntries(Func<EngineOwners.CharacterGaoShunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoShunBoundaryMobileEntries Create(Func<EngineOwners.CharacterGaoShunBoundaryMobileRuntime> resolve) => new(resolve);
}
