#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime))]
internal sealed partial class CharacterZhaoYunOlGod2016Entries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> _resolve;
    private CharacterZhaoYunOlGod2016Entries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunOlGod2016Entries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> resolve) => new(resolve);
}
