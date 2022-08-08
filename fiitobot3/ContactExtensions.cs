using fiitobot.Services;

namespace fiitobot
{
    public static class ContactExtensions
    {
        public static ContactDetailsLevel GetDetailsLevelFor(this Contact contact, Contact contactViewer)
        {
            if (contactViewer == null) return ContactDetailsLevel.No;
            return contactViewer.Type switch
            {
                ContactType.Administration => ContactDetailsLevel.Minimal | ContactDetailsLevel.Contacts | ContactDetailsLevel.LinksToFiitTeamFiles | ContactDetailsLevel.Marks | ContactDetailsLevel.SecretNote,
                ContactType.Teacher => ContactDetailsLevel.Minimal | ContactDetailsLevel.Contacts | ContactDetailsLevel.Marks,
                ContactType.Student when contactViewer.AdmissionYear == contact.AdmissionYear => ContactDetailsLevel.Minimal | ContactDetailsLevel.Contacts,
                ContactType.Student => ContactDetailsLevel.Minimal,
                _ when contactViewer.TgId == 33598070 => ContactDetailsLevel.Iddqd, // @xoposhiy id
                _ => ContactDetailsLevel.No
            };
        }
    }
}