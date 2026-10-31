#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed partial class CharacterCaoZhiLeEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhiLeRuntime> _resolve;
    private CharacterCaoZhiLeEntries(Func<EngineOwners.CharacterCaoZhiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhiLeEntries Create(Func<EngineOwners.CharacterCaoZhiLeRuntime> resolve) => new(resolve);
}
