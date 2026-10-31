using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Durable private courses, rather than pending choices. Commands rebuild
    // these immutable accepted orders on replay; public facts reveal no future animal.
    private ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.MobileWulingCourse> _mobileWulingCourses => ref _characterHuaTuoMobileGod.ReferenceToMobileWulingCourses;
}
