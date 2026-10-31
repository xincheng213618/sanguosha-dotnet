#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangYiBoundaryMobileRuntime))]
internal sealed partial class CharacterWangYiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterWangYiBoundaryMobileRuntime> _resolve;
    private CharacterWangYiBoundaryMobileEntries(Func<EngineOwners.CharacterWangYiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYiBoundaryMobileEntries Create(Func<EngineOwners.CharacterWangYiBoundaryMobileRuntime> resolve) => new(resolve);
}
