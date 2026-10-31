#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangXiuMouRuntime))]
internal sealed partial class CharacterZhangXiuMouEntries
{
    private readonly Func<EngineOwners.CharacterZhangXiuMouRuntime> _resolve;
    private CharacterZhangXiuMouEntries(Func<EngineOwners.CharacterZhangXiuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXiuMouEntries Create(Func<EngineOwners.CharacterZhangXiuMouRuntime> resolve) => new(resolve);
}
