namespace CardGame.Core;

/// <summary>The native source3382 entry rule is consumed by initial and later form-entry features.</summary>
public static class ShanheSourceDongdangRules
{
    public const string SkillId = "shanhe:source-dongdang-3382";
    public const string BindingId = "second-form-entry";
    public const string InitialBindingId = "initial-entry";

    public static void ValidateDefinition(ContentSkillDefinition definition)
    {
        if (definition.Id != SkillId || definition.Program is not null ||
            definition.Tags != SkillTag.Locked || definition.ExecutionForms != SkillExecutionForm.Trigger ||
            definition.ImplementationStatus != SkillImplementationStatus.Complete)
            throw new InvalidOperationException("Source Dongdang requires its registered, complete locked native entry skill, without a program substitute.");
    }
}

public sealed record ShanheSourceDongdangActivatedEvent(int BossSeat, CardConversionSource Source,
    string PreviousGeneralId, string GeneralId, int InterruptedSeat, int TurnNumber) : IGameEvent;

public sealed record ShanheSourceDongdangInitialEntryEvent(int OwnerSeat, CardConversionSource Source,
    string GeneralId, int PreviousStartingSeat) : IGameEvent;
