#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanTanYuanShangOlRuntime))]
internal sealed partial class CharacterYuanTanYuanShangOlEntries
{
    private readonly Func<EngineOwners.CharacterYuanTanYuanShangOlRuntime> _resolve;
    private CharacterYuanTanYuanShangOlEntries(Func<EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanTanYuanShangOlEntries Create(Func<EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve) => new(resolve);
}
