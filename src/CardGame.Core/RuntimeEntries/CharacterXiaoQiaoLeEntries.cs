#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed partial class CharacterXiaoQiaoLeEntries
{
    private readonly Func<EngineOwners.CharacterXiaoQiaoLeRuntime> _resolve;
    private CharacterXiaoQiaoLeEntries(Func<EngineOwners.CharacterXiaoQiaoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoLeEntries Create(Func<EngineOwners.CharacterXiaoQiaoLeRuntime> resolve) => new(resolve);
}
