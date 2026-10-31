#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeGuoMobileRuntime))]
internal sealed partial class CharacterZhugeGuoMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhugeGuoMobileRuntime> _resolve;
    private CharacterZhugeGuoMobileEntries(Func<EngineOwners.CharacterZhugeGuoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoMobileEntries Create(Func<EngineOwners.CharacterZhugeGuoMobileRuntime> resolve) => new(resolve);
}
