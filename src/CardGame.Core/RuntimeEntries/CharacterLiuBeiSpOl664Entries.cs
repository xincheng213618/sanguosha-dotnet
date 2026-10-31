#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuBeiSpOl664Runtime))]
internal sealed partial class CharacterLiuBeiSpOl664Entries
{
    private readonly Func<EngineOwners.CharacterLiuBeiSpOl664Runtime> _resolve;
    private CharacterLiuBeiSpOl664Entries(Func<EngineOwners.CharacterLiuBeiSpOl664Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuBeiSpOl664Entries Create(Func<EngineOwners.CharacterLiuBeiSpOl664Runtime> resolve) => new(resolve);
}
