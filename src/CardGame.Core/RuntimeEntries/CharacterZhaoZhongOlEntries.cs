#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed partial class CharacterZhaoZhongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _resolve;
    private CharacterZhaoZhongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoZhongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> resolve) => new(resolve);
}
