namespace CardGame.Core;

internal sealed record NativeTrickContinuationRead(
    CardUseFrame Use, Card Card, ProgramTrickContinuation Continuation);

internal interface INativeTrickContinuations : IProgramRuntimeOperations
{
    void ValidateIssuance(long useId, CardActionContext action, ProgramCardContinuation stage,
        ProgramTrickContinuation continuation);
    bool Matches(ProgramCardTriggerWindowFrame window);
    NativeTrickContinuationRead Read(ProgramCardTriggerWindowFrame window);
    NativeTrickContinuationRead Read(ActualUseTargetWindowFrame window);
}
