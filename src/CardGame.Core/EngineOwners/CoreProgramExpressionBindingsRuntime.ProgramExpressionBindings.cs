#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramExpressionBindingsRuntime
{
    void IProgramExpressionCaptureHost.CaptureCards(ProgramSkillFrame frame, string name, ProgramExpression expression)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (ProgramExpressions.EvaluateCardSet(expression, _kernelCoreGameEngineProgramExpressionsOperations.CreateProgramExpressionWorld(active)) is not { } cards)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "冻结牌集合无法计算，技能剩余步骤取消。");
            return;
        }
        bindings.SetProgramCardSet(active.Id, name, cards.Select(card => card.CardId).ToArray(),
            SkillProgramCardSetVisibility.Private,
            cards.Select(card => card.CapturedLocation ?? zones.GetLocation(card.CardId)).ToArray(),
            isReadOnlySnapshot: true,
            sourceEntrySequences: cards.Select(card => card.SourceEntrySequence).ToArray());
    }

    void IProgramExpressionCaptureHost.Capture(ProgramSkillFrame frame, string name, ProgramExpression expression)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (ProgramExpressions.EvaluateInt(expression, _kernelCoreGameEngineProgramExpressionsOperations.CreateProgramExpressionWorld(active))is not { } value)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "冻结表达式无法计算，技能剩余步骤取消。");
            return;
        }

        _coreProgramExpressionsOperations.CaptureProgramExpressionNumber(active.Id, name, value);
    }
}
