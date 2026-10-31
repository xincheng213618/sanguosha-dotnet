#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime))]
internal sealed partial class CharacterXiangChongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime> _resolve;
    private CharacterXiangChongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiangChongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime> resolve) => new(resolve);
}
