#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed partial class CharacterZhugeGuoLeEntries
{
    private readonly Func<EngineOwners.CharacterZhugeGuoLeRuntime> _resolve;
    private CharacterZhugeGuoLeEntries(Func<EngineOwners.CharacterZhugeGuoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoLeEntries Create(Func<EngineOwners.CharacterZhugeGuoLeRuntime> resolve) => new(resolve);
}
