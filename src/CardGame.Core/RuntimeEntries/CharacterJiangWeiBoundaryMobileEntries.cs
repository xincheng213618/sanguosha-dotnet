#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiangWeiBoundaryMobileRuntime))]
internal sealed partial class CharacterJiangWeiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> _resolve;
    private CharacterJiangWeiBoundaryMobileEntries(Func<EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiBoundaryMobileEntries Create(Func<EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> resolve) => new(resolve);
}
