namespace CardGame.Core;

/// <summary>A continuous relation supplied by a currently effective skill source.</summary>
public sealed record AttackRangeRelationDeclaration(string Id, ProgramExpression While, Role? OwnerRole = null);

internal interface IAttackRangeQueries : IProgramSelectionQuery
{
    bool HasRelation(int actor, int target);
    bool IsWithinAttackRange(int actor, int target, int? excludedEquipmentId = null,
        EquipmentSlot? excludedEquipmentSlot = null);
}

internal interface IAttackRangeExpressionWorlds : IProgramSelectionQuery
{
    IProgramExpressionWorld Create(int sourceOwner, int actor, int target);
}
