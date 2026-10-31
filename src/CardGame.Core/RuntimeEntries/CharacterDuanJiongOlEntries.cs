#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDuanJiongOlRuntime))]
internal sealed partial class CharacterDuanJiongOlEntries
{
    private readonly Func<EngineOwners.CharacterDuanJiongOlRuntime> _resolve;
    private CharacterDuanJiongOlEntries(Func<EngineOwners.CharacterDuanJiongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuanJiongOlEntries Create(Func<EngineOwners.CharacterDuanJiongOlRuntime> resolve) => new(resolve);
}
