#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed partial class CharacterXiaoQiaoMouOlEntries
{
    private readonly Func<EngineOwners.CharacterXiaoQiaoMouOlRuntime> _resolve;
    private CharacterXiaoQiaoMouOlEntries(Func<EngineOwners.CharacterXiaoQiaoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoMouOlEntries Create(Func<EngineOwners.CharacterXiaoQiaoMouOlRuntime> resolve) => new(resolve);
}
