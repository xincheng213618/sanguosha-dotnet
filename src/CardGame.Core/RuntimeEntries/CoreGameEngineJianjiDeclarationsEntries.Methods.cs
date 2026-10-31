#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJianjiDeclarationsEntries
{
    internal bool HuangQuanDeclarationObserverSuffix(int rootIndex) => _resolve().HuangQuanDeclarationObserverSuffix(rootIndex);
    internal bool IsJianjiDeclarationChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().IsJianjiDeclarationChild(root, child);
    internal bool IsJianjiDeclarationObserverEdge(int index) => _resolve().IsJianjiDeclarationObserverEdge(index);
    internal bool IsJianjiDeclarationQuery(CharacterState actor, int? selectedId) => _resolve().IsJianjiDeclarationQuery(actor, selectedId);
    internal bool JianjiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().JianjiFinalizedAfterCategoryTargetWindow(query);
    internal bool JianjiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().JianjiNormalizeImplicitDrawTwoTargetWindow(query);
    internal bool MatchesJianjiDeclaration(ProgramSkillFrame root, CardDeclarationFrame declaration) => _resolve().MatchesJianjiDeclaration(root, declaration);
    internal bool TryReturnJianjiDeclaration(CardDeclarationFrame declaration, bool success) => _resolve().TryReturnJianjiDeclaration(declaration, success);
    internal bool HasJianjiNativeUseCapability { get => _resolve().HasJianjiNativeUseCapability; }
}
