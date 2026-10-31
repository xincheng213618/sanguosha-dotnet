#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouDunBoundaryMobileRuntime))]
internal sealed partial class CharacterXiahouDunBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> _resolve;
    private CharacterXiahouDunBoundaryMobileEntries(Func<EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouDunBoundaryMobileEntries Create(Func<EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve) => new(resolve);
}
