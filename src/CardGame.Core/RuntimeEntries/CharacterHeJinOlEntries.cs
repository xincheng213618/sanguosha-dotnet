#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHeJinOlRuntime))]
internal sealed partial class CharacterHeJinOlEntries
{
    private readonly Func<EngineOwners.CharacterHeJinOlRuntime> _resolve;
    private CharacterHeJinOlEntries(Func<EngineOwners.CharacterHeJinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeJinOlEntries Create(Func<EngineOwners.CharacterHeJinOlRuntime> resolve) => new(resolve);
}
