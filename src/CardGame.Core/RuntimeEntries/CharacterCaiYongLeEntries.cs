#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed partial class CharacterCaiYongLeEntries
{
    private readonly Func<EngineOwners.CharacterCaiYongLeRuntime> _resolve;
    private CharacterCaiYongLeEntries(Func<EngineOwners.CharacterCaiYongLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiYongLeEntries Create(Func<EngineOwners.CharacterCaiYongLeRuntime> resolve) => new(resolve);
}
