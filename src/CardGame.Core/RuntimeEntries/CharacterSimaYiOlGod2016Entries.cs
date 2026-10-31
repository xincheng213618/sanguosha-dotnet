#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime))]
internal sealed partial class CharacterSimaYiOlGod2016Entries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime> _resolve;
    private CharacterSimaYiOlGod2016Entries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiOlGod2016Entries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime> resolve) => new(resolve);
}
