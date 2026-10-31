#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicSlashTraitsEntries
{
    internal global::CardGame.Core.PublicSlashTraits CreatePublicSlashTraits(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind, bool? declaredColor, global::CardGame.Core.GameSnapshot? ownerView) => _resolve().CreatePublicSlashTraits(actor, action, kind, declaredColor, ownerView);
}
