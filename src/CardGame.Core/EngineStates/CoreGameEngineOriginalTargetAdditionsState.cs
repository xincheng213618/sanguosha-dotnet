#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineOriginalTargetAdditionsState : Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess
{
    private List<OriginalTargetAdditionGrant> _originalTargetAdditionGrants = [];
    internal ref readonly List<OriginalTargetAdditionGrant> ReferenceToOriginalTargetAdditionGrants => ref _originalTargetAdditionGrants;

    ref readonly List<OriginalTargetAdditionGrant> Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants => ref _originalTargetAdditionGrants;
}
