#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangQuanMouRuntime))]
internal sealed partial class CharacterHuangQuanMouEntries
{
    private readonly Func<EngineOwners.CharacterHuangQuanMouRuntime> _resolve;
    private CharacterHuangQuanMouEntries(Func<EngineOwners.CharacterHuangQuanMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanMouEntries Create(Func<EngineOwners.CharacterHuangQuanMouRuntime> resolve) => new(resolve);
}
