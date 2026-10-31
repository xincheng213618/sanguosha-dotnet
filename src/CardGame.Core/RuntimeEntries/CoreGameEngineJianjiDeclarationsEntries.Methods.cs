#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJianjiDeclarationsEntries
{
    internal bool HuangQuanDeclarationObserverSuffix(int rootIndex) => _resolve().HuangQuanDeclarationObserverSuffix(rootIndex);
    internal bool IsJianjiDeclarationChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().IsJianjiDeclarationChild(root, child);
    internal bool IsJianjiDeclarationObserverEdge(int index) => _resolve().IsJianjiDeclarationObserverEdge(index);
    internal bool IsJianjiDeclarationQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => _resolve().IsJianjiDeclarationQuery(actor, selectedId);
    internal bool JianjiFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().JianjiFinalizedAfterCategoryTargetWindow(query);
    internal bool JianjiNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().JianjiNormalizeImplicitDrawTwoTargetWindow(query);
    internal bool MatchesJianjiDeclaration(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.CardDeclarationFrame declaration) => _resolve().MatchesJianjiDeclaration(root, declaration);
    internal bool TryReturnJianjiDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success) => _resolve().TryReturnJianjiDeclaration(declaration, success);
    internal bool HasJianjiNativeUseCapability { get => _resolve().HasJianjiNativeUseCapability; }
}
