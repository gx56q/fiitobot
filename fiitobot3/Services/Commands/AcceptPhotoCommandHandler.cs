using System.Linq;
using System.Threading.Tasks;

namespace fiitobot.Services.Commands
{
    public class RejectPhotoCommandHandler : IChatCommandHandler
    {
        private readonly IPresenter presenter;
        private readonly IBotDataRepository repo;
        private readonly IPhotoRepository photoRepository;
        private readonly long reviewerChatId;

        public RejectPhotoCommandHandler(IPresenter presenter, IBotDataRepository repo, IPhotoRepository photoRepository, long reviewerChatId)
        {
            this.presenter = presenter;
            this.repo = repo;
            this.photoRepository = photoRepository;
            this.reviewerChatId = reviewerChatId;
        }

        public string[] Synonyms => new[] { "/reject_photo" };
        public AccessRight[] AllowedFor => new[] { AccessRight.Admin, AccessRight.Staff, AccessRight.Student, };
        public async Task HandlePlainText(string text, long fromChatId, Contact sender, bool silentOnNoResults = false)
        {
            if (fromChatId != reviewerChatId) return;
            var parts = text.Split(" ");
            if (parts.Length != 2) return;
            if (!long.TryParse(parts[1], out var contactTgId)) return;
            var person = repo.GetData().AllContacts.FirstOrDefault(c => c.Contact.TgId == contactTgId);
            if (person == null) return;
            var success = await photoRepository.RejectPhoto(contactTgId);
            if (success)
            {
                await presenter.SayPhotoRejected(person.Contact, sender, fromChatId);
                await presenter.SayPhotoRejected(person.Contact, null, person.Contact.TgId);
            }
        }
    }

    public class AcceptPhotoCommandHandler : IChatCommandHandler
    {
        private readonly IPresenter presenter;
        private readonly IBotDataRepository repo;
        private readonly IPhotoRepository photoRepository;
        private readonly long reviewerChatId;

        public AcceptPhotoCommandHandler(IPresenter presenter, IBotDataRepository repo, IPhotoRepository photoRepository, long reviewerChatId)
        {
            this.presenter = presenter;
            this.repo = repo;
            this.photoRepository = photoRepository;
            this.reviewerChatId = reviewerChatId;
        }

        public string[] Synonyms => new[] { "/accept_photo" };
        public AccessRight[] AllowedFor => new[] { AccessRight.Admin, AccessRight.Staff, AccessRight.Student, };
        public async Task HandlePlainText(string text, long fromChatId, Contact sender, bool silentOnNoResults = false)
        {
            if (fromChatId != reviewerChatId) return;
            var parts = text.Split(" ");
            if (parts.Length != 2) return;
            if (!long.TryParse(parts[1], out var contactTgId)) return;
            var person = repo.GetData().AllContacts.FirstOrDefault(c => c.Contact.TgId == contactTgId);
            if (person == null) return;
            var success = await photoRepository.AcceptPhoto(contactTgId);
            if (success)
            {
                await presenter.SayPhotoAccepted(person.Contact, sender, fromChatId);
                await presenter.SayPhotoAccepted(person.Contact, null, person.Contact.TgId);
            }
        }
    }
}