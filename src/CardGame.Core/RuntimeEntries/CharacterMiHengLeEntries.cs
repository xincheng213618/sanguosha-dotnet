#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMiHengLeRuntime))]
internal sealed partial class CharacterMiHengLeEntries
{
    private readonly Func<EngineOwners.CharacterMiHengLeRuntime> _resolve;
    private CharacterMiHengLeEntries(Func<EngineOwners.CharacterMiHengLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMiHengLeEntries Create(Func<EngineOwners.CharacterMiHengLeRuntime> resolve) => new(resolve);
}
