#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineOriginalTargetAdditionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.OriginalTargetAdditionGrant> _originalTargetAdditionGrants = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OriginalTargetAdditionGrant> ReferenceToOriginalTargetAdditionGrants => ref _originalTargetAdditionGrants;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OriginalTargetAdditionGrant> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants => ref _originalTargetAdditionGrants;
}
