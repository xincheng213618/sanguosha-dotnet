#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed partial class CharacterHuangYueyingOlMouEntries
{
    private readonly Func<EngineOwners.CharacterHuangYueyingOlMouRuntime> _resolve;
    private CharacterHuangYueyingOlMouEntries(Func<EngineOwners.CharacterHuangYueyingOlMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangYueyingOlMouEntries Create(Func<EngineOwners.CharacterHuangYueyingOlMouRuntime> resolve) => new(resolve);
}
