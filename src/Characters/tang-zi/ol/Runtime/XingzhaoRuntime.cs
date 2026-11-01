namespace CardGame.Core;

internal interface ISelectionWoundedQueries : IProgramSelectionQuery
{
    bool IdentityConcealed(int seat);
    bool IsRuleWounded(int seat);
}
