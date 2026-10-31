#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed partial class CharacterZhugeLiangWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _resolve;
    private CharacterZhugeLiangWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> resolve) => new(resolve);
}
