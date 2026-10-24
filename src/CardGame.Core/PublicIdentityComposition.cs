namespace CardGame.Core;

/// <summary>Public mode composition, never a mapping from seats to hidden roles.</summary>
public sealed record PublicIdentityComposition(int Lords, int Loyalists, int Rebels, int Renegades);
