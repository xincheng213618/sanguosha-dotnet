#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuLieMouRuntime))]
internal sealed partial class CharacterHuLieMouEntries
{
    private readonly Func<EngineOwners.CharacterHuLieMouRuntime> _resolve;
    private CharacterHuLieMouEntries(Func<EngineOwners.CharacterHuLieMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuLieMouEntries Create(Func<EngineOwners.CharacterHuLieMouRuntime> resolve) => new(resolve);
}
