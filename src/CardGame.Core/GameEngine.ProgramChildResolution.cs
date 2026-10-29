namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Business rules qualify and pay their own operation. Every admitted child
    // then follows these frame-type contracts, independent of the issuing skill.
    private bool ProgramChildResolutionEdge(int index)
    {
        if (index < 1 || index >= _resolutionStack.Count) return false;
        var parent = _resolutionStack[index - 1];
        var child = _resolutionStack[index];
        if (parent is ProgramSkillFrame && child is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame ||
            child is ProgramSkillFrame && parent is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame)
            return TypedDamagePostEventEdge(parent, child) &&
                (child is not CardsMovedTriggerWindowFrame movement || movement.Batch.Movements.All(_cardMovements.Contains));
        return _resolutionStack.HasIssuedLink(index);
    }

    private bool HasOwnedProgramChildSubtree(long rootFrameId)
    {
        var root = _resolutionStack.FindIndex(frame => frame.Id == rootFrameId);
        if (root < 0) return false;
        for (var index = root + 1; index < _resolutionStack.Count; index++)
            if (!ProgramChildResolutionEdge(index)) return false;
        return true;
    }
}
