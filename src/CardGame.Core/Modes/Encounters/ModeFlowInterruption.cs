namespace CardGame.Core;

/// <summary>A typed synchronous transition request; never a saved or independently pending resolution.</summary>
public abstract record ModeFlowTransitionRequest;

/// <summary>
/// Retires the current synchronous caller before it can continue an obsolete mode flow.
/// The step runner handles this signal inside the same accepted command and never serializes it.
/// </summary>
public sealed class ModeFlowInterruption : Exception
{
    public ModeFlowInterruption(string handlerId, ModeFlowTransitionRequest request)
        : base("A mode transition interrupted the current synchronous flow.")
    {
        HandlerId = new ContentId(handlerId).Value;
        Request = request ?? throw new ArgumentNullException(nameof(request));
    }

    public string HandlerId { get; }
    public ModeFlowTransitionRequest Request { get; }
}

public interface IModeFlowInterruptionHandler
{
    string InterruptionHandlerId { get; }
    void CompleteInterruption(ModeFlowTransitionRequest request);
}
