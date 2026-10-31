#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMouEntries
{
    private readonly Func<EngineOwners.CharacterCaoShuangMouRuntime> _resolve;
    private CharacterCaoShuangMouEntries(Func<EngineOwners.CharacterCaoShuangMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangMouEntries Create(Func<EngineOwners.CharacterCaoShuangMouRuntime> resolve) => new(resolve);
}
