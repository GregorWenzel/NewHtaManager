using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class PublicationViewModel : ViewModelBase
    {
        private ObservableCollection<PublicationAuthor> authorList;
        public ObservableCollection<PublicationAuthor> AuthorList
        {
            get => authorList;
            set => SetProperty(ref authorList, value);
        }

        private string abstractText;
        public string AbstractText
        {
            get => abstractText;
            set => SetProperty(ref abstractText, value);
        }

        private string doi;
        public string DOI
        {
            get => doi;
            set => SetProperty(ref doi, value);
        }

        private string issue;
        public string Issue
        {
            get => issue;
            set => SetProperty(ref issue, value);
        }

        private string journalShortTitle;
        public string JournalShortTitle
        {
            get => journalShortTitle;
            set => SetProperty(ref journalShortTitle, value);
        }

        private string journalFullTitle;
        public string JournalFullTitle
        {
            get => journalFullTitle;
            set => SetProperty(ref journalFullTitle, value);
        }

        private ObservableCollection<PublicationMeshHeading> meshHeadingList;
        public ObservableCollection<PublicationMeshHeading> MeshHeadingList
        {
            get => meshHeadingList;
            set => SetProperty(ref meshHeadingList, value);
        }

        private string nctId;
        public string NctId
        {
            get => nctId;
            set => SetProperty(ref nctId, value);
        }

        private string pagination;
        public string Pagination
        {
            get => pagination;
            set => SetProperty(ref pagination, value);
        }

        private string pmid;
        public string PMID
        {
            get => pmid;
            set => SetProperty(ref pmid, value);
        }

        private PublicationAbstract publicationAbstract;
        public PublicationAbstract PublicationAbstract
        {
            get => publicationAbstract;
            set => SetProperty(ref publicationAbstract, value);
        }

        private DateTime publicationDate;
        public DateTime PublicationDate
        {
            get => publicationDate;
            set => SetProperty(ref publicationDate, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private PublicationType type;
        public PublicationType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private string volume;
        public string Volume
        {
            get => volume;
            set => SetProperty(ref volume, value);
        }

        public string Citation
        {
            get
            {
                return ($"{AuthorString}. {Title}. {JournalShortTitle}. {PublicationDate.Year}; {Volume}{IssueString}:{Pagination}.").Replace("..", ".");
            }
        }

        private string AuthorString
        {
            get
            {
                if (AuthorList.Count > 3)
                {
                    return string.Join(", ", AuthorList.Select(item => item.ToString()).Take(3)) + ", et al";
                }
                else
                {
                    return string.Join(", ", AuthorList.Select(item => item.ToString()));
                }
            }
        }

        private string IssueString
        {
            get => string.IsNullOrEmpty(Issue) ? "" : $"({Issue})";
        }

        public static explicit operator PublicationViewModel(Publication publication)
        {
            return new PublicationViewModel
            {
                AbstractText = publication.AbstractText,
                AuthorList = new ObservableCollection<PublicationAuthor>(publication.AuthorList),
                DOI = publication.DOI,
                Issue = publication.Issue,
                JournalShortTitle = publication.JournalShortTitle,
                JournalFullTitle = publication.JournalFullTitle,
                MeshHeadingList = new ObservableCollection<PublicationMeshHeading>(publication.MeshHeadingList),
                NctId = publication.NctId,
                Pagination = publication.Pagination,
                PMID = publication.PMID,
                PublicationAbstract = publication.PublicationAbstract,
                PublicationDate = publication.PublicationDate,
                Title = publication.Title,
                Type = publication.Type,
                Volume = publication.Volume
            };
        }

        public static explicit operator Publication(PublicationViewModel publicationViewModel)
        {
            return new Publication
            {
                AbstractText = publicationViewModel.AbstractText,
                AuthorList = publicationViewModel.AuthorList.ToList(),
                DOI = publicationViewModel.DOI,
                Issue = publicationViewModel.Issue,
                JournalShortTitle = publicationViewModel.JournalShortTitle,
                JournalFullTitle = publicationViewModel.JournalFullTitle,
                MeshHeadingList = publicationViewModel.MeshHeadingList.ToList(),
                NctId = publicationViewModel.NctId,
                Pagination = publicationViewModel.Pagination,
                PMID = publicationViewModel.PMID,
                PublicationAbstract = publicationViewModel.PublicationAbstract,
                PublicationDate = publicationViewModel.PublicationDate,
                Title = publicationViewModel.Title,
                Type = publicationViewModel.Type,
                Volume = publicationViewModel.Volume
            };
        }

        public PublicationViewModel(Publication publication)
        {
            this.AbstractText = publication.AbstractText;
            this.AuthorList = publication.AuthorList is object ? new ObservableCollection<PublicationAuthor>(publication.AuthorList) : new ObservableCollection<PublicationAuthor>();
            this.DOI = publication.DOI;
            this.Issue = publication.Issue;
            this.JournalShortTitle = publication.JournalShortTitle;
            this.JournalFullTitle = publication.JournalFullTitle;
            this.MeshHeadingList = publication.MeshHeadingList is object ? new ObservableCollection<PublicationMeshHeading>(publication.MeshHeadingList) : new ObservableCollection<PublicationMeshHeading>();
            this.NctId = publication.NctId;
            this.Pagination = publication.Pagination;
            this.PMID = publication.PMID;
            this.PublicationAbstract = publication.PublicationAbstract;
            this.PublicationDate = publication.PublicationDate;
            this.Title = publication.Title;
            this.Type = publication.Type;
            this.Volume = publication.Volume;
        }

        public PublicationViewModel()
        {

        }
    }
}
