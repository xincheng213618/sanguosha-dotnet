#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed partial class CharacterGuanYuRuntimeEntries
{
    private readonly Func<EngineOwners.CharacterGuanYuRuntimeRuntime> _resolve;
    private CharacterGuanYuRuntimeEntries(Func<EngineOwners.CharacterGuanYuRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuRuntimeEntries Create(Func<EngineOwners.CharacterGuanYuRuntimeRuntime> resolve) => new(resolve);
}
