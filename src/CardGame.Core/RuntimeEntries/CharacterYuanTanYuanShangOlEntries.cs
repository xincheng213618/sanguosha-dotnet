#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime))]
internal sealed partial class CharacterYuanTanYuanShangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> _resolve;
    private CharacterYuanTanYuanShangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanTanYuanShangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve) => new(resolve);
}
