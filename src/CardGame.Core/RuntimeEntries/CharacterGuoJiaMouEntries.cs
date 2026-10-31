#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoJiaMouRuntime))]
internal sealed partial class CharacterGuoJiaMouEntries
{
    private readonly Func<EngineOwners.CharacterGuoJiaMouRuntime> _resolve;
    private CharacterGuoJiaMouEntries(Func<EngineOwners.CharacterGuoJiaMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaMouEntries Create(Func<EngineOwners.CharacterGuoJiaMouRuntime> resolve) => new(resolve);
}
