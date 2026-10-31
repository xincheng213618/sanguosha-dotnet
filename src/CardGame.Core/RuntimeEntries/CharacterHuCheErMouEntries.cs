#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed partial class CharacterHuCheErMouEntries
{
    private readonly Func<EngineOwners.CharacterHuCheErMouRuntime> _resolve;
    private CharacterHuCheErMouEntries(Func<EngineOwners.CharacterHuCheErMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuCheErMouEntries Create(Func<EngineOwners.CharacterHuCheErMouRuntime> resolve) => new(resolve);
}
