namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ProgramLegalityRouter? _programLegality;
    private ProgramLegalityRouter ProgramLegality => _programLegality ??= new(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsDefinitions.ProgramLegalityMetadata.Value, this, ResolveRuntimeHandlerTarget);
    // The journal rebuilds these counters through accepted commands. They are
    // keyed by content identities, never by translated presentation text.
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programUses => ref _skillPrograms.ReferenceToProgramUses;
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses => ref _skillPrograms.ReferenceToProgramPhaseUses;
    private ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses => ref _skillPrograms.ReferenceToProgramContributionUses;
}

public sealed record ProgramSkillStartedEvent(long FrameId, int OwnerSeat, string SkillId, string ActivationId) : IGameEvent;
public sealed record ProgramSkillResolvedEvent(long FrameId, int OwnerSeat, string SkillId, string ActivationId, bool Completed) : IGameEvent;
public sealed record ProgramSkillHpLostEvent(long FrameId, string SkillId, int TargetSeat, int Amount, int RemainingHp) : IGameEvent
{
    /// <summary>The exact public card use whose completion triggered this loss; absent for unrelated or earlier payments.</summary>
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? CompletedCardUseResolutionId { get; init; }
}

public sealed record ProgramSkillContributionResolvedEvent(long ActionId, int ProviderSeat, int SkillOwnerSeat, string SkillId, string ContributionId, int CardId, CardKind CardKind, Suit CardSuit, [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] string? SkillInstanceId = null) : IGameEvent;
