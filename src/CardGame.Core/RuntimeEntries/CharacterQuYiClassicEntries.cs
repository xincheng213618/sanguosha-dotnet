#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQuYiClassicRuntime))]
internal sealed partial class CharacterQuYiClassicEntries
{
    private readonly Func<EngineOwners.CharacterQuYiClassicRuntime> _resolve;
    private CharacterQuYiClassicEntries(Func<EngineOwners.CharacterQuYiClassicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuYiClassicEntries Create(Func<EngineOwners.CharacterQuYiClassicRuntime> resolve) => new(resolve);
}
