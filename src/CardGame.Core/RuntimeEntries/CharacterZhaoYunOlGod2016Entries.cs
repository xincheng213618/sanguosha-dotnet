#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoYunOlGod2016Runtime))]
internal sealed partial class CharacterZhaoYunOlGod2016Entries
{
    private readonly Func<EngineOwners.CharacterZhaoYunOlGod2016Runtime> _resolve;
    private CharacterZhaoYunOlGod2016Entries(Func<EngineOwners.CharacterZhaoYunOlGod2016Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunOlGod2016Entries Create(Func<EngineOwners.CharacterZhaoYunOlGod2016Runtime> resolve) => new(resolve);
}
