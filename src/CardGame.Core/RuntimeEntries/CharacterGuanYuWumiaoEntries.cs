#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed partial class CharacterGuanYuWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterGuanYuWumiaoRuntime> _resolve;
    private CharacterGuanYuWumiaoEntries(Func<EngineOwners.CharacterGuanYuWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuWumiaoEntries Create(Func<EngineOwners.CharacterGuanYuWumiaoRuntime> resolve) => new(resolve);
}
