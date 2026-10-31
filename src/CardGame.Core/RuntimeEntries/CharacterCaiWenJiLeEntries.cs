#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed partial class CharacterCaiWenJiLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _resolve;
    private CharacterCaiWenJiLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiWenJiLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> resolve) => new(resolve);
}
