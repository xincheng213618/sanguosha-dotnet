#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime))]
internal sealed partial class CharacterXiaoQiaoBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> _resolve;
    private CharacterXiaoQiaoBoundaryMobileEntries(Func<EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoBoundaryMobileEntries Create(Func<EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> resolve) => new(resolve);
}
