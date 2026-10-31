#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZouShiLeRuntime))]
internal sealed partial class CharacterZouShiLeEntries
{
    private readonly Func<EngineOwners.CharacterZouShiLeRuntime> _resolve;
    private CharacterZouShiLeEntries(Func<EngineOwners.CharacterZouShiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZouShiLeEntries Create(Func<EngineOwners.CharacterZouShiLeRuntime> resolve) => new(resolve);
}
