#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(SkillOwnershipWindowRuntime))]
internal sealed class CoreSkillOwnershipWindowsSkillOwnershipWindowAdapter(Func<SkillOwnershipWindowRuntime> targetFactory) : ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations
{
    private readonly Func<SkillOwnershipWindowRuntime> _targetFactory = targetFactory;
    public SkillOwnershipWindowRuntime Get() => this._targetFactory();
}
