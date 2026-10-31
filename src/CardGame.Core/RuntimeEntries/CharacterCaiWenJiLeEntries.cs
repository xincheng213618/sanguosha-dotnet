#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed partial class CharacterCaiWenJiLeEntries
{
    private readonly Func<EngineOwners.CharacterCaiWenJiLeRuntime> _resolve;
    private CharacterCaiWenJiLeEntries(Func<EngineOwners.CharacterCaiWenJiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiWenJiLeEntries Create(Func<EngineOwners.CharacterCaiWenJiLeRuntime> resolve) => new(resolve);
}
