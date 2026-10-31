namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.OriginalTargetAdditionGrant> _originalTargetAdditionGrants => ref _originalTargetAdditions.ReferenceToOriginalTargetAdditionGrants;
}
