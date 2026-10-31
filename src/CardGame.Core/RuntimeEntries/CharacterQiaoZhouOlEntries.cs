#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed partial class CharacterQiaoZhouOlEntries
{
    private readonly Func<EngineOwners.CharacterQiaoZhouOlRuntime> _resolve;
    private CharacterQiaoZhouOlEntries(Func<EngineOwners.CharacterQiaoZhouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQiaoZhouOlEntries Create(Func<EngineOwners.CharacterQiaoZhouOlRuntime> resolve) => new(resolve);
}
