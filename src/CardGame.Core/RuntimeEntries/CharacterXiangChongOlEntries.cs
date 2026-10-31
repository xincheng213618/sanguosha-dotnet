#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiangChongOlRuntime))]
internal sealed partial class CharacterXiangChongOlEntries
{
    private readonly Func<EngineOwners.CharacterXiangChongOlRuntime> _resolve;
    private CharacterXiangChongOlEntries(Func<EngineOwners.CharacterXiangChongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiangChongOlEntries Create(Func<EngineOwners.CharacterXiangChongOlRuntime> resolve) => new(resolve);
}
