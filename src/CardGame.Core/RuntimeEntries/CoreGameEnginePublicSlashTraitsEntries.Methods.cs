#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicSlashTraitsEntries
{
    internal PublicSlashTraits CreatePublicSlashTraits(CharacterState actor, LegalAction action, CardKind kind, bool? declaredColor, GameSnapshot? ownerView) => _resolve().CreatePublicSlashTraits(actor, action, kind, declaredColor, ownerView);
}
