#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeJinMobileRuntime))]
internal sealed partial class CharacterZhugeJinMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhugeJinMobileRuntime> _resolve;
    private CharacterZhugeJinMobileEntries(Func<EngineOwners.CharacterZhugeJinMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeJinMobileEntries Create(Func<EngineOwners.CharacterZhugeJinMobileRuntime> resolve) => new(resolve);
}
