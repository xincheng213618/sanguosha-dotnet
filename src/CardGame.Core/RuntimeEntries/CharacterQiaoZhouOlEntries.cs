#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed partial class CharacterQiaoZhouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> _resolve;
    private CharacterQiaoZhouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQiaoZhouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> resolve) => new(resolve);
}
