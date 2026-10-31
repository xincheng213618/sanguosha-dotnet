namespace CardGame.Core;

internal abstract record EngineNotification;

internal sealed record LogNotification(GameLogEntry Entry) : EngineNotification;

internal sealed record AiThoughtNotification(AiThoughtRecord Thought) : EngineNotification;

internal sealed record AiGeneralThoughtNotification(AiGeneralThought Thought) : EngineNotification;

internal sealed record CardMovedNotification(CardMovementRecord Movement) : EngineNotification;

internal sealed record EventNotification(EventEnvelope Event) : EngineNotification;
