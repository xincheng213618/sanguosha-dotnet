#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMengHuoBoundaryMobileRuntime))]
internal sealed partial class CharacterMengHuoBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterMengHuoBoundaryMobileRuntime> _resolve;
    private CharacterMengHuoBoundaryMobileEntries(Func<EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoBoundaryMobileEntries Create(Func<EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve) => new(resolve);
}
