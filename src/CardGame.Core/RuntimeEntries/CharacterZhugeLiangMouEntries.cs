#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed partial class CharacterZhugeLiangMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _resolve;
    private CharacterZhugeLiangMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> resolve) => new(resolve);
}
