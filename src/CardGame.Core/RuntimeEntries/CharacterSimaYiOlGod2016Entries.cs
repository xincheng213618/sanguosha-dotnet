#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaYiOlGod2016Runtime))]
internal sealed partial class CharacterSimaYiOlGod2016Entries
{
    private readonly Func<EngineOwners.CharacterSimaYiOlGod2016Runtime> _resolve;
    private CharacterSimaYiOlGod2016Entries(Func<EngineOwners.CharacterSimaYiOlGod2016Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiOlGod2016Entries Create(Func<EngineOwners.CharacterSimaYiOlGod2016Runtime> resolve) => new(resolve);
}
