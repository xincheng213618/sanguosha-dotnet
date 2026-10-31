#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed partial class CharacterCaiYongLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _resolve;
    private CharacterCaiYongLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiYongLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> resolve) => new(resolve);
}
