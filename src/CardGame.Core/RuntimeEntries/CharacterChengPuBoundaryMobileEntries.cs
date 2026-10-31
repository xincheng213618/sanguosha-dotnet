#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengPuBoundaryMobileRuntime))]
internal sealed partial class CharacterChengPuBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterChengPuBoundaryMobileRuntime> _resolve;
    private CharacterChengPuBoundaryMobileEntries(Func<EngineOwners.CharacterChengPuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengPuBoundaryMobileEntries Create(Func<EngineOwners.CharacterChengPuBoundaryMobileRuntime> resolve) => new(resolve);
}
