#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaChaoBoundaryMobileRuntime))]
internal sealed partial class CharacterMaChaoBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterMaChaoBoundaryMobileRuntime> _resolve;
    private CharacterMaChaoBoundaryMobileEntries(Func<EngineOwners.CharacterMaChaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoBoundaryMobileEntries Create(Func<EngineOwners.CharacterMaChaoBoundaryMobileRuntime> resolve) => new(resolve);
}
