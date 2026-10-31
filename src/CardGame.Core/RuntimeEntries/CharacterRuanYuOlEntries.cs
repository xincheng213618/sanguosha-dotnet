#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed partial class CharacterRuanYuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _resolve;
    private CharacterRuanYuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanYuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> resolve) => new(resolve);
}
