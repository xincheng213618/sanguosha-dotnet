#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed partial class CharacterGuanHaiOlEntries
{
    private readonly Func<EngineOwners.CharacterGuanHaiOlRuntime> _resolve;
    private CharacterGuanHaiOlEntries(Func<EngineOwners.CharacterGuanHaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanHaiOlEntries Create(Func<EngineOwners.CharacterGuanHaiOlRuntime> resolve) => new(resolve);
}
