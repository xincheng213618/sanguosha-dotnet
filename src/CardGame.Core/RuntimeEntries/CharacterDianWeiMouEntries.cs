#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDianWeiMouRuntime))]
internal sealed partial class CharacterDianWeiMouEntries
{
    private readonly Func<EngineOwners.CharacterDianWeiMouRuntime> _resolve;
    private CharacterDianWeiMouEntries(Func<EngineOwners.CharacterDianWeiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiMouEntries Create(Func<EngineOwners.CharacterDianWeiMouRuntime> resolve) => new(resolve);
}
