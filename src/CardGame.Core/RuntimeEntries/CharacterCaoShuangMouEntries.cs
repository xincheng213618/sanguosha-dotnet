#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMouRuntime> _resolve;
    private CharacterCaoShuangMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMouRuntime> resolve) => new(resolve);
}
