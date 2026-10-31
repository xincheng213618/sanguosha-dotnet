#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuYiBoundaryMobileRuntime))]
internal sealed partial class CharacterWuYiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterWuYiBoundaryMobileRuntime> _resolve;
    private CharacterWuYiBoundaryMobileEntries(Func<EngineOwners.CharacterWuYiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuYiBoundaryMobileEntries Create(Func<EngineOwners.CharacterWuYiBoundaryMobileRuntime> resolve) => new(resolve);
}
