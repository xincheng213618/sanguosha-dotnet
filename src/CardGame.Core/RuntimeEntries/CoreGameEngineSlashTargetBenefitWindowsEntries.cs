#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetBenefitWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitWindowsEntries(Func<EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitWindowsEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> resolve) => new(resolve);
}
