#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanShaoBoundaryMobileRuntime))]
internal sealed partial class CharacterYuanShaoBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> _resolve;
    private CharacterYuanShaoBoundaryMobileEntries(Func<EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanShaoBoundaryMobileEntries Create(Func<EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> resolve) => new(resolve);
}
