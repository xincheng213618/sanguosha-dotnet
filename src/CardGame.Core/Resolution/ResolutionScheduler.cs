namespace CardGame.Core;

internal interface IResolutionLifecycle
{
    void CardUsePushed(CardUseFrame frame, CardActionContext action);
    void BeforeDamageCompleted(DamageFrame frame);
    void RecoveryCompleted(RecoveryFrame frame);
}

/// <summary>Owns the frame stack and its identity, parent and completion boundaries.</summary>
internal sealed class ResolutionScheduler(IResolutionLifecycle lifecycle) : INativeFrameRoutingFacts
{
    internal FrameStore Frames { get; } = new();
    internal ResolutionIdentitySequence Identities { get; } = new();

    internal ResolutionFrame? Top => Frames.LastOrDefault();
    internal ResolutionFrame? Find(long id) => Frames.FirstOrDefault(frame => frame.Id == id);
    internal ResolutionFrame? ParentOf(long id)
    {
        var index = Frames.FindLastIndex(frame => frame.Id == id);
        return index > 0 ? Frames[index - 1] : null;
    }

    internal ResolutionFrame? ChildOf(long id)
    {
        var index = Frames.FindLastIndex(frame => frame.Id == id);
        return index >= 0 && index + 1 < Frames.Count ? Frames[index + 1] : null;
    }

    internal IReadOnlyList<T> OfType<T>() where T : ResolutionFrame =>
        Array.AsReadOnly(Frames.OfType<T>().ToArray());

    internal ProgramSkillFrame ActiveProgram(long id) =>
        Top is ProgramSkillFrame frame && frame.Id == id
            ? frame
            : throw new InvalidOperationException($"Program frame {id} is not active.");

    int INativeFrameRoutingFacts.FrameCount => Frames.Count;
    int INativeFrameRoutingFacts.LastFrameIndex(long id) => Frames.FindLastIndex(frame => frame.Id == id);
    ResolutionFrame INativeFrameRoutingFacts.FrameAt(int index) => Frames[index];

    internal void Push(ResolutionFrame frame)
    {
        // A zero judgment parent denotes the ordinary turn boundary. Program
        // contexts may name completed facts and retain their typed return checks.
        var parentId = frame switch
        {
            NativeDrawAttemptFrame child => child.Context.ParentFrameId,
            IdentityAppearanceFrame child => child.Receipt.ParentFrameId ?? 0,
            RequestedDeckBasicFrame child => child.ParentFrameId,
            DrawFundedDistinctBasicFrame child => child.Payment.RequestFrameId ?? child.Payment.ParentFrameId ?? 0,
            ChainedStateBasicFrame child => child.Payment.RequestFrameId ?? child.Payment.ParentFrameId ?? 0,
            ActualUseTargetWindowFrame child => child.ParentFrameId,
            SlashTargetBenefitWindowFrame child => child.ParentFrameId,
            FirstSlashDuelLegacyWindowFrame child => child.ParentFrameId,
            IProgramChildResolutionFrame child => child.ParentFrameId,
            PindianRevealWindowFrame child => child.ParentFrameId,
            WuLuKangPindianFrame child => child.ParentFrameId,
            DouyuWindowFrame child => child.ParentFrameId,
            SlashTargetPenaltyWindowFrame child => child.ParentFrameId,
            CardDeclarationFrame child => child.Return.ParentFrameId,
            CardDeclarationChallengeFrame child => child.ParentFrameId,
            ResponseWindowFrame child => child.ParentFrameId,
            JudgmentFrame child => child.ParentFrameId,
            DamageFrame child => child.ParentFrameId,
            DamageTriggerWindowFrame child => child.ParentFrameId,
            RecoveryFrame child => child.ParentFrameId,
            RecoveryReplacementFrame child => child.ParentFrameId,
            DrawPhaseObligationFrame child => child.ParentFrameId ?? 0,
            ActualPhaseEndFrame child => child.ParentFrameId ?? 0,
            DyingFrame child => child.ParentFrameId,
            DeathFrame child => child.ParentFrameId,
            NullificationWindowFrame child => child.ParentFrameId,
            TargetCardSelectionFrame child => child.ParentFrameId,
            _ => 0
        };
        if (parentId > 0 && !Frames.Any(parent => parent.Id == parentId))
            throw new InvalidOperationException("A runtime child cannot outlive its owning frame.");
        Frames.PushOwned(frame);
        if (frame is CardUseFrame use && use.Action is { Type: CardActionType.Use } action)
            lifecycle.CardUsePushed(use, action);
    }

    internal void Replace(long expectedFrameId, ResolutionFrame next)
    {
        if (next.Id != expectedFrameId)
            throw new InvalidOperationException("A runtime write changed its selected owner identity.");
        Frames.Replace(next);
    }

    internal void ReplaceTop(ResolutionFrame next)
    {
        if (Frames.LastOrDefault()?.Id != next.Id)
            throw new InvalidOperationException("Only the active runtime frame may write its top continuation.");
        Frames.Replace(next);
    }

    internal void Complete(long frameId, ResolutionFrameKind expectedKind)
    {
        if (Frames.LastOrDefault()?.NativeDraw is not null)
            throw new InvalidOperationException("A producer cannot complete with an unconsumed native draw request.");
        if (Frames.LastOrDefault() is { PendingRecoveryAttempts.Count: > 0 })
            throw new InvalidOperationException("A producer cannot complete with unconsumed recovery attempts.");
        if (Frames.LastOrDefault()?.PaidFactionRequestCostRecovery is not null)
            throw new InvalidOperationException("A producer cannot complete with an unconsumed paid faction request cost.");
        if (expectedKind == ResolutionFrameKind.Damage && Frames.LastOrDefault() is DamageFrame damage && damage.Id == frameId)
            lifecycle.BeforeDamageCompleted(damage);
        var completed = Frames.CompleteTop(frameId, expectedKind);
        if (completed is RecoveryFrame recovery)
            lifecycle.RecoveryCompleted(recovery);
    }
}
