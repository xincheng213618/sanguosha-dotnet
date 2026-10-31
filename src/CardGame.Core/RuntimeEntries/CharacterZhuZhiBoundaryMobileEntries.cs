#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuZhiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuZhiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> _resolve;
    private CharacterZhuZhiBoundaryMobileEntries(Func<EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuZhiBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> resolve) => new(resolve);
}
